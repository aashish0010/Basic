using Basic.Application.Function;
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

        [Route("~/api/user/gettokenvalues")]

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





        [Route("~/api/user/gettoken")]
        [HttpGet]
        public async Task<IActionResult> GenerateForgetToken(string username, string email, string link)
        {
            var data = await _unitOfWork.ForgetPasswordService.GenerateForgetProcessid(email);
            if (data.Code == 200)
            {
                EmailMessage ema = new EmailMessage();
                ema.EmailToName = username;
                ema.EmailBody = $@"
                    <p style=""font-size: 180%"">Dear {username}</p>
                    <br>
                    <br>
                    <hr>
                    <b style=""margin-left:150px; font-size:110%"">A request has been received to change the password for your account</b>
                    <br>
                    <br>
                    <button href=""facebook.com/{data.Message}/{NormalFunctions.encrypt(email)}"" style=""margin-left:250px;background-color: #4CAF50;
                      border: none;
                      color: white;
                      padding: 15px 32px;
                      text-align: center;
                      text-decoration: none;
                      display: inline-block;
                      font-size: 16px;"">Reset Password</button>
                      <footer style="" text-align: center;margin-top:20px;margin-down:20px;
                      padding: 3px;
                      background-color: #e7e9eb;
                      color: black;"">Token will expire for the 10 min</footer>
                      <p>Thanks,</p>";

                ema.EmailSubject = "Forget Password";
                ema.EmailToId = email;
                bool emailres = await _unitOfWork.emailService.SendEmail(ema);
                if (emailres == true)
                {
                    data.Message = "Message Send Successfully";
                    return Ok(data);
                }
                else
                {
                    return BadRequest(new CommonResponse()
                    {
                        Code = 400,
                        Message = "Email Send Failed"
                    });
                }


            }
            return BadRequest(data);
        }

        [Route("~/api/user/verifyforgetpassword")]
        [HttpGet]
        public IActionResult PasswordChange(string email, string processid)
        {
            var data = _unitOfWork.ForgetPasswordService.VerifyUser(NormalFunctions.Decrypt(email), processid);
            if (data.Code == 200)
            {
                return Ok(data);
            }
            return BadRequest(data);
        }

    }
}
