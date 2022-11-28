using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public UserController(IUnitOfWork unitOfWork, ITokenService service)
        {
            _tokenService = service;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("login")]
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

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Register Register)
        {
            var data = await _unitOfWork.LoginRegisterService.Register(Register);
            return Ok(data.FirstOrDefault());
        }

        [HttpGet]
        [Route("refreshtoken")]
        private Tokens RefreshToken(string token)
        {
            var tokendata = _tokenService.GetPrincipalFromExpiredToken(token);
            string user = tokendata.Identity.Name;
            string role = tokendata.FindFirst("Role").Value;
            Tokens reftok = _tokenService.GenerateRefreshToken(user, tokendata.FindFirst("Email")?.Value, role, tokendata.FindFirst("Isadmin")?.Value);
            return reftok;
        }

        [HttpGet]
        [Authorize]
        [Route("tokenverifyandrefresh")]
        public IActionResult TokenVerifyAndRefresh()
        {

            var re = Request;

            string token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
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
                CurrentUser = _tokenService.GetDataFromToken(token)
            };
            return Ok(obj);

        }


        [HttpGet]
        [Route("emailforgetpassword")]
        public async Task<IActionResult> EmailForgetPassword(string username, string email, string link)
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
                    <b style=""font-size:110%"">A request has been received to change the password for your account</b>
                    <br>
                    <br>
                    <a href=""{link}/{data.Message}/{NormalFunctions.encrypt(email)}"" style=""margin-left:250px;background-color: #4CAF50;
                      border: none;
                      color: white;
                      padding: 15px 32px;
                      text-align: center;
                      text-decoration: none;
                      display: inline-block;
                      font-size: 16px;"">Reset Password</a>
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


        [HttpGet]
        [Route("verifyforgetpassword")]
        public IActionResult VerfiyForgetPassword(string email, string processid)
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
