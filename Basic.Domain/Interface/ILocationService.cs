using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface ILocationService
    {
        //void StoreLocation(string username, double startLat, double startLng);
        Task<DistanceDetail> GetAllUserWithLocation(double startLat, double startLng, string username);
        Task<CommonResponse> ApproveUser(string username);
        Task<IEnumerable<string>> FriendList(string username);
        CommonResponse Unfriend(string username, string targetusername);
    }
}
