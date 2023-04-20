using Basic.Application.Function;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
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
            if (data == null)
            {
                return Unauthorized(new CommonResponse
                {
                    Code = 401,
                    Message = "Unauthorized"
                });
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
            string interests = string.Empty;
            int i = 0;
            if (Register.Interests.Count() > 1)
            {
                foreach (var items in Register.Interests)
                {
                    if (i == 0)
                        interests = items;

                    else
                        interests = interests + "," + items;

                    i++;
                }
            }
            else
            {
                interests = Register.Interests.FirstOrDefault();
            }
            var data = await _unitOfWork.LoginRegisterService.Register(Register, interests);
            if (data.FirstOrDefault()?.Code == 0)
            {
                Tokens token = _tokenService.GenerateToken(Register.Username, Register.Email, "user", "y");
                return Ok(token);
            }
            else
                return BadRequest(data.FirstOrDefault());


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

        [Route("tokenverifyandrefresh")]
        public IActionResult TokenVerifyAndRefresh(string token)
        {
            if (token == null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "token not found"
                });
            }

            if (_tokenService.IsTokenValid(token) == true)
            {

                dynamic obj = new
                {
                    Token = token
                };
                return Ok(obj);
            }
            else
            {
                if (_tokenService.CheckTheTokenTime(token) == false)
                {
                    var newtoken = RefreshToken(token);
                    token = newtoken.Token;
                    dynamic obj = new
                    {
                        Token = token
                    };
                    return Ok(obj);
                }

                return Unauthorized(new
                {
                    code = 201,
                    message = "Unauthorized User..!!!"
                });
            }




        }


        [HttpGet]
        [Route("forgetpasswordrequest")]
        public async Task<IActionResult> EmailForgetPassword(string email)
        {
            var data = await _unitOfWork.ForgetPasswordService.GenerateForgetProcessid(email);
            if (data.Code == 200)
            {
                EmailMessage ema = new EmailMessage();
                ema.EmailToName = data.Username;
                ema.EmailBody = $@"<p style=""font-size: 180%"">Dear {data.Username}</p>
                    <br>
                    <p style=""text-align: center;color:red;font-size:200%""><b>{data.Otp}</b></p>
                    <br>
                    <hr>
                    <b style=""font-size:110%"">A request has been received to change the password for your account</b>
                    <br>
                    <br>
                   
                      <footer style="" text-align: center;margin-top:20px;margin-down:20px;
                      padding: 3px;
                      background-color: #e7e9eb;
                      color: black;"">Token will expire for the 10 min</footer>
                      <p>Thanks,</p>";

                ema.EmailSubject = "Forget Password";
                ema.EmailToId = email;
                //bool emailres = await _unitOfWork.emailService.SendEmail(ema);
                bool emailres = true;
                if (emailres == true)
                {

                    return Ok(new
                    {
                        Code = 200,
                        Message = "Otp send succesfully",
                        Processid = data.ProcessId,
                        Otp = data.Otp
                    });
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
        public IActionResult VerfiyForgetPassword(VerfiyForgetPassword password)
        {
            var data = _unitOfWork.ForgetPasswordService.VerifyUser(NormalFunctions.Decrypt(password.Processid), password.Processid, password.Otp);
            if (data.Code == 200)
            {
                _unitOfWork.ForgetPasswordService.ChangePassword(NormalFunctions.Decrypt(password.Processid), password.Password);
                return Ok(data);
            }
            return BadRequest(data);
        }

    }
}
