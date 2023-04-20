using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IDashBoardService
    {
        DashBoard GetUserClaimsData(string token);
        dynamic GetImage(string username, string flag = null);
        dynamic GetUserDetail(string token);
    }
}
