using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginRegisterController(IUnitOfWork unitOfWork, ITokenService service)
        {
            _tokenService = service;
            _unitOfWork = unitOfWork;
        }
        [Route("~/api/user/login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var data = await _unitOfWork.LoginRegisterService.Login(login);
            if ((data.Count() != 0) && (data.FirstOrDefault().Code == 0))
            {
                var token = _tokenService.GenerateToken(data.FirstOrDefault().Username, data.FirstOrDefault().Email, data.FirstOrDefault().Role, data.FirstOrDefault().Isactive);
                return Ok(token);
            }

            return Unauthorized(new CommonResponse
            {
                Code = 401,
                Message = "Unauthorize User"
            });

        }
        [Route("~/api/user/register")]
        [HttpPost]
        public async Task<IActionResult> Register(Register Register)
        {
            var data = await _unitOfWork.LoginRegisterService.Register(Register);
            return Ok(data.FirstOrDefault());
        }
        [Route("~/api/user/refreshtoken")]
        [HttpPost]
        public IActionResult RefreshToken()
        {
            var re = Request;
            var token = re.Headers["token"].FirstOrDefault();
            if (token == null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "token not found"
                });
            }
            var tokendata = _tokenService.GetPrincipalFromExpiredToken(token);
            if (tokendata != null)
            {
                string user = tokendata.Identity.Name;
                string role = tokendata.FindFirst("Role").Value;

                if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(role))
                {
                    var reftok = _tokenService.GenerateRefreshToken(user, tokendata.FindFirst("Email")?.Value, role, tokendata.FindFirst("Isadmin")?.Value);
                    return Ok(reftok);
                }
                return Unauthorized(new CommonResponse
                {
                    Code = 401,
                    Message = "Unauthorize User"
                });
            }
            else
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "Invalid Token"
                });
            }



        }

        [Route("~/api/user/gettokenvalues")]

        [HttpGet]
        public IActionResult GetTokenValues(string roles)
        {
            var re = Request;

            var token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            if (token == null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "token not found"
                });
            }
            return Ok(_tokenService.GetSpecificTokenData(token, roles));

        }
    }
}
