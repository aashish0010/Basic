using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageManagerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private TokenHandlerController _tokenHandlerController;
        public ImageManagerController(IUnitOfWork unitOfWork, ApplicationDbContext context, TokenHandlerController tokenHandlerController)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _tokenHandlerController = tokenHandlerController;
        }
        // [Authorize]
        [Route("imageupload")]
        [HttpPost]
        public async Task<IActionResult> Post()
        {

            var userdetail = _tokenHandlerController.GetUserDetail(Request);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }
            var re = Request;

            var token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];

            UserDocReq imgdetail = new UserDocReq();

            if (Request.Form.Files.Count > 0)
            {
                CommonResponse data1 = new CommonResponse();
                var data = Request.Form.Files.Count;
                foreach (var file in Request.Form.Files)
                {
                    imgdetail.Image = file;

                    data1 = _unitOfWork.imageHandlerService.UploadImage(token, imgdetail);

                }
                if (data1.Code == 200)
                {
                    return Ok(data1);
                }
                else
                {
                    return BadRequest(new CommonResponse
                    {
                        Code = 400,
                        Message = "Technical Error!!!"
                    });
                }

            }

            return BadRequest(new CommonResponse
            {
                Code = 400,
                Message = "Technical Error!!!"
            });




        }

        [Route("makeprofilepic")]
        [HttpGet]
        public IActionResult MakeProfilePic(Uri url)
        {
            var re = Request;
            var userdetail = _tokenHandlerController.GetUserDetail(re);
            if (userdetail.common.Code != 200)
            {
                return BadRequest(userdetail.common);
            }


            var token = re.Headers["Authorization"].FirstOrDefault().Split(' ')[1];

            bool dec = _unitOfWork.imageHandlerService.MakeProfilepic(url, token);
            if (dec == true)
            {
                return Ok(new CommonResponse
                {
                    Code = 200,
                    Message = "Update Successfully..!!!"
                });
            }
            else
            {
                return BadRequest(new CommonResponse
                {
                    Code = 400,
                    Message = "Update Failed..!!!"
                });
            }
        }


    }

}
