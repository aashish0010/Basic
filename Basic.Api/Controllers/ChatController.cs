using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //[HttpPost]
        //[Route("chat")]
        //public async Task<IActionResult> Chat(string user, string message)
        //{
        //    var re = Request;
        //    var token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];
        //    if (token == null)
        //    {
        //        return BadRequest(new CommonResponse
        //        {
        //            Code = 400,
        //            Message = "token not found"
        //        });
        //    }
        //    var data = _unitOfWork.dashBoardService.GetUserClaimsData(token);
        //    ChatHub chatHub = new ChatHub();
        //    await chatHub.SendToUser(data.UserName, user, message);
        //    return Ok("");
        //}
    }
}
