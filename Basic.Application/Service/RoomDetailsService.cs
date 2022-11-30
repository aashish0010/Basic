using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;

namespace Basic.Application.Service
{
    public class RoomDetailsService : IRoomDetailsInterface
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
    }
}
