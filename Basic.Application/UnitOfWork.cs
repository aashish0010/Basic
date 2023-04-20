using Basic.Application.Service;
using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;

namespace Basic.Application
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public LoginRegisterService LoginRegisterService => new LoginRegisterService();

        public DashBoardService dashBoardService => new DashBoardService(_context);

        public ForgetPasswordService ForgetPasswordService => new ForgetPasswordService(_context);

        public EmailService emailService => new EmailService(_context);

        public RoomDetailsService roomDetailsService => new RoomDetailsService(_context);

        public ImageHandlerService imageHandlerService => new ImageHandlerService(_context);

        public CommonService commonService => new CommonService(_context);

        public LocationService locationService => new LocationService();
    }
}
