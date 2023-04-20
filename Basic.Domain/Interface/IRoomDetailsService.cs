using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IRoomDetailsService
    {
        void CreateRoomType(RoomType room);
        dynamic GetRoomType();
    }
}
