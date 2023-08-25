using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]/v{version:apiVersion}")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }
        [HttpPost]
        [Route("signin")]
        public IActionResult SignIn([FromBody] UserVO userVO) 
        {
            if (userVO == null) return BadRequest("Invalid client request");

            var token = _loginBusiness.ValidateLoginCredentials(userVO);

            if (token == null) return Unauthorized();

            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid client request");

            var token = _loginBusiness.ValidateTokenCredentials(tokenVO);

            if (token == null) return BadRequest("Invalid client request");

            return Ok(token);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")] // É necessário estar autenticado para sabe qual token revogar
        public IActionResult Revoke()
        {
            var userName = User.Identity.Name;
            var revokeResult = _loginBusiness.RevokeToken(userName);

            if (!revokeResult)
            {
                return BadRequest("Invalid Client Request");
            }

            return NoContent();
        }
    }
}
