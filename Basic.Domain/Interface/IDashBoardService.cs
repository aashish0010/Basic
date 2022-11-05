using Basic.Domain.Entity;

namespace Basic.Domain.Interface
{
    public interface IDashBoardService
    {
        Uservalidate GetUserClaimsData(string token);
    }
}
