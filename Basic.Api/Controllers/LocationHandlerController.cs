using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationHandlerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private TokenHandlerController _tokenHandlerController;
        public LocationHandlerController(IUnitOfWork unitOfWork, TokenHandlerController tokenHandlerController)
        {
            _unitOfWork = unitOfWork;
            _tokenHandlerController = tokenHandlerController;
        }
        //private const double EarthRadius = 6371.0; // in kilometers
        [HttpPost]
        [Route("findmatch")]
        public async Task<IActionResult> FindMatch(MatchFind find)
        {

            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }

            var data12 = await _unitOfWork.locationService.GetAllUserWithLocation(find.Latitude, find.Longitude, userdetail.userdetail.UserName);
            if (data12 == null || data12.Code == 001)
            {
                return BadRequest(new CommonResponse()
                {
                    Code = 400,
                    Message = "Nearest User Not Found"
                });
            }

            if (data12.Receiver == userdetail.userdetail.UserName)
            {
                data12.Receiver = data12.Sender;
            }

            //var SenderImage = _unitOfWork.dashBoardService.GetImage(data12.Sender, "y");
            var ReceiveriImage = _unitOfWork.dashBoardService.GetImage(data12.Receiver, "y");
            return Ok(new
            {
                ReceiverDetail = _unitOfWork.dashBoardService.GetReceiverDetail(data12.Receiver),
                ReceiverImageDetail = ReceiveriImage,
                Distance = data12.DistanceInKm,
                SenderApprove = (String.IsNullOrEmpty(data12.SenderApprove) ? "n" : data12.SenderApprove),
                ReceiverApprove = (String.IsNullOrEmpty(data12.ReceiverApprove) ? "n" : data12.ReceiverApprove),
                Message = data12.Message
            });
            //return Ok(data12);
        }

        [HttpPost]
        [Route("approvematch")]
        public async Task<IActionResult> ApproveMatch()
        {
            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }
            var data12 = await _unitOfWork.locationService.ApproveUser(userdetail.userdetail.UserName);
            return Ok(data12);
        }
        [HttpPost]
        [Route("friendlist")]
        public async Task<IActionResult> FriendList()
        {
            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }
            var friendlst = await _unitOfWork.locationService.FriendList(userdetail.userdetail.UserName);
            if (friendlst != null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "No Any Friends"
                });
            }
            return Ok(friendlst);
        }
        [HttpPost]
        [Route("unfriend")]
        public IActionResult UnFriend(string targetusername)
        {
            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }
            CommonResponse common = _unitOfWork.locationService.Unfriend(userdetail.userdetail.UserName, targetusername);
            if (common != null)
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "No Any Friends"
                });
            }
            return Ok(common);
        }
    }
}
