using Basic.Application.Service;
using Basic.Domain.Interface;
using Microsoft.Extensions.Configuration;

namespace Basic.Application
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public LoginRegisterService LoginRegisterService => new LoginRegisterService();

        public DashBoardService dashBoardService => new DashBoardService(_configuration);
    }
}
