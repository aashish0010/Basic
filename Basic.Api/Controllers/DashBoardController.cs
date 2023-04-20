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
        private readonly ITokenService _tokenService;
        private TokenHandlerController _tokenHandlerController;
        public DashBoardController(IUnitOfWork unitOfWork, ITokenService tokenService, TokenHandlerController tokenHandlerController)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _tokenHandlerController = tokenHandlerController;
        }
        [Route("dashboard")]
        [HttpGet]
        [Authorize]

        public IActionResult DashBoardDetail()
        {
            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }

            var imgdetail = _unitOfWork.dashBoardService.GetImage(userdetail.userdetail.UserName, "y");
            return Ok(new
            {
                UserDetail = userdetail.userdetail,
                ImageDetail = imgdetail
            });

        }
        [Route("userdetail")]
        [HttpGet]
        [Authorize]

        public IActionResult UserInfo()
        {

            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }
            // var imgdetail = _unitOfWork.dashBoardService.GetImage(data.UserName);
            return Ok(new
            {
                UserDetail = userdetail.userdetail,
                //ImageDetail = imgdetail
            });

        }
    }
}
