using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;

namespace Basic.Application.Service
{
    public class RoomDetailsService : IRoomDetailsService
    {
        private readonly ApplicationDbContext context;

        public RoomDetailsService(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void CreateRoomType(RoomType room)
        {
            context.RoomType.Add(room);
            context.SaveChanges();
        }

        public dynamic GetRoomType()
        {
            var data = context.RoomType.ToList().Select(x => new
            {
                x.Description,
                x.Type,
                x.Createby
            });
            return data;
        }
    }
}
