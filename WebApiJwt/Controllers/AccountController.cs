using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Common;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]/[action]")]
    [Microsoft.AspNetCore.Mvc.ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;

        public AccountController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        [HttpGet]
        public ActionResult<string> GetToken()
        {
            return _jwtHelper.CreateToken();
        }

        [Authorize]
        [HttpGet]
        public ActionResult<string> GetTest()
        {
            return "Test Authorize";
        }
    }
}
