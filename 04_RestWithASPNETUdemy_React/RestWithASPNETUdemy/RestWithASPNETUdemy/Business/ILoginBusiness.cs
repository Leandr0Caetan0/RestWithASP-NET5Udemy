using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateLoginCredentials(UserVO user);
        TokenVO ValidateTokenCredentials(TokenVO tokenVO);
        bool RevokeToken(string userName);
    }
}
