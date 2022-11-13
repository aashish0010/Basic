using Basic.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Basic.Infrastracture.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ForgetPassword> EmailRequest { get; set; }
        public DbSet<Config> Config { get; set; }

    }
}
