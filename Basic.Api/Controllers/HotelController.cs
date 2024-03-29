﻿using AutoMapper;
using Basic.Domain.DTO;
using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public HotelController(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        [HttpPost]
        [Route("createroomtype")]
        public IActionResult CreateRoomType(RoomtypeDto room)
        {
            if (room.id != 0)
            {
                return Ok(new
                {
                    code = 200,
                    message = "Update Successfully..!!!"
                });
            }
            else
            {
                var roomdata = mapper.Map<RoomType>(room);
                unitOfWork.roomDetailsService.CreateRoomType(roomdata);

                return Ok(new
                {
                    Code = 200,
                    Message = "Added Successfully..!!!"
                });
            }

        }
        [HttpPost]
        [Route("getroomtype")]
        public IActionResult GetRoomType()
        {
            var data = unitOfWork.roomDetailsService.GetRoomType();
            return Ok(data);
        }

    }
}
