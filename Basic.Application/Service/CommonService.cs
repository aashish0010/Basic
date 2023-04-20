using Basic.Domain.Interface;
using Basic.Infrastracture.Entity;
using System.Collections;

namespace Basic.Application.Service
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext _context;
        public CommonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public dynamic GetStaticValues()
        {
            ArrayList arr = new ArrayList();
            string desc = string.Empty;
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            var types = _context.StaticValuesType.ToList();
            foreach (var type in types)
            {
                dict.Add(type.Description, _context.StaticValues.Where(x => x.Sno == type.Id).Select(x => x.StaticValue).ToList());
            }
            return dict;
        }
    }
}
