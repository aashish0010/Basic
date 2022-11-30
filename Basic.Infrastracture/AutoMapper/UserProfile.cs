using AutoMapper;
using Basic.Domain.DTO;
using Basic.Domain.Entity;

namespace Basic.Infrastracture.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RoomtypeDto, RoomType>();
        }
    }
}
