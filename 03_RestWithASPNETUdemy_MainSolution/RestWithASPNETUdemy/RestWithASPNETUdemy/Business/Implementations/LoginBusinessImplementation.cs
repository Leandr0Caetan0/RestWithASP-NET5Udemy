using Microsoft.IdentityModel.JsonWebTokens;
using RestWithASPNETUdemy.Configurations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Repository;
using RestWithASPNETUdemy.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;
        private IUserRepository _repository;
        private ITokenService _token;

        public LoginBusinessImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService token)
        {
            _configuration = configuration;
            _repository = repository;
            _token = token;
        }

        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }

        public TokenVO ValidateLoginCredentials(UserVO userVO)
        {
            var user = _repository.ValidateUserCredentials(userVO);

            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var accesstoken = _token.GenerateAccessToken(claims);
            var refreshToken = _token.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configuration.DaysToExpiry);

            _repository.UpdateUser(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accesstoken,
                refreshToken
                );
        }

        public TokenVO ValidateTokenCredentials(TokenVO tokenVO)
        {
            var accesstoken = tokenVO.AccessToken;
            var refreshToken = tokenVO.RefreshToken;

            var principal = _token.GetPrincipalFromExpiredToken(accesstoken);

            var userName = principal.Identity.Name;

            var user = _repository.ValidateUserNameCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            accesstoken = _token.GenerateAccessToken(principal.Claims);
            refreshToken = _token.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.UpdateUser(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accesstoken,
                refreshToken
                );
        }
    }
}
