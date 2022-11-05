using Microsoft.EntityFrameworkCore;

namespace Basic.Infrastracture.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
