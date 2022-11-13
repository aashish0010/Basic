using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Authorization;
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
                Tokens token = _tokenService.GenerateToken(data.FirstOrDefault().Username, data.FirstOrDefault().Email, data.FirstOrDefault().Role, data.FirstOrDefault().Isactive);
                return Ok(token);
            }

            return Unauthorized(new CommonResponse
            {
                Code = 401,
                Message = data.FirstOrDefault().Message
            });

        }
        [Route("~/api/user/register")]
        [HttpPost]
        public async Task<IActionResult> Register(Register Register)
        {
            var data = await _unitOfWork.LoginRegisterService.Register(Register);
            return Ok(data.FirstOrDefault());
        }

        [HttpGet]
        public Tokens RefreshToken(string token)
        {
            var tokendata = _tokenService.GetPrincipalFromExpiredToken(token);
            string user = tokendata.Identity.Name;
            string role = tokendata.FindFirst("Role").Value;
            Tokens reftok = _tokenService.GenerateRefreshToken(user, tokendata.FindFirst("Email")?.Value, role, tokendata.FindFirst("Isadmin")?.Value);
            return reftok;
        }

        [Route("~/api/user/refreshtoken")]

        [HttpGet]
        [Authorize]
        public IActionResult GetTokenValues(string roles)
        {
            string token = string.Empty;
            var re = Request;

            token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            if (token == null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "token not found"
                });
            }

            if (_tokenService.CheckTokenIsValid(token) == false)
            {
                var newtoken = RefreshToken(token);
                token = newtoken.Token;
            }

            dynamic obj = new
            {
                CurrentUser = _tokenService.GetSpecificTokenData(token, roles)
            };
            return Ok(obj);

        }

        [Route("~/api/user/userdetail")]
        [HttpGet]
        public async Task<IActionResult> GenerateForgetToken(string email)
        {
            await _unitOfWork.ForgetPasswordService.GenerateForgetProcessid(email);
            return Ok();
        }

        [Route("~/api/user/passwordchange")]
        [HttpGet]
        public async Task<IActionResult> PasswordChange(string email, string processid)
        {
            await _unitOfWork.ForgetPasswordService.VerifyUser(email, processid);
            return Ok();
        }

    }
}
