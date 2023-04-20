using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenHandlerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TokenHandlerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [NonAction]
        public DashBoardWithCommon GetUserDetail(HttpRequest re)
        {
            //var re = Request;

            var token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
            if (token == null)
            {
                return new DashBoardWithCommon
                {
                    common = new CommonResponse
                    {
                        Code = 400,
                        Message = "token not found"
                    },
                    userdetail = null
                };
            }
            else
            {
                return new DashBoardWithCommon
                {
                    common = new CommonResponse
                    {
                        Code = 200,
                        Message = "User Found Successfully"
                    },
                    userdetail = _unitOfWork.dashBoardService.GetUserClaimsData(token)
                };
            }
            // return ;
        }
    }
}
