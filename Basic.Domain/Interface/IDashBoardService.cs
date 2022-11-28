using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IDashBoardService
    {
        DashBoard GetUserClaimsData(string token);
    }
}
