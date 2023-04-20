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
        public DbSet<User> tbl_user { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<OtpManager> OtpManager { get; set; }
        public DbSet<UserDoc> UserDoc { get; set; }
        public DbSet<Common> StaticValues { get; set; }
        public DbSet<CommonType> StaticValuesType { get; set; }
        public DbSet<Location> LocationHandler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomType>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<OtpManager>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<UserDoc>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Common>()
                .HasNoKey();
            modelBuilder.Entity<CommonType>()
                .HasNoKey();
        }
    }
}
