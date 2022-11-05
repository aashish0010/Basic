

using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashBoardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Route("~/api/dashboard/userinfo")]
        [HttpGet]

        public IActionResult UserInfo()
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
            var data = _unitOfWork.dashBoardService.GetUserClaimsData(token);

            return Ok(data);

        }
    }
}
