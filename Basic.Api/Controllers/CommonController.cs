using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("staticvalues")]
        public IActionResult GetValues()
        {
            return Ok(_unitOfWork.commonService.GetStaticValues());
        }
    }
}
