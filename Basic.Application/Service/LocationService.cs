using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Dapper;
using Dapper;

namespace Basic.Application.Service
{

    public class LocationService : ILocationService
    {
        private const double EarthRadius = 6371.0; // in kilometers
        public async Task<DistanceDetail> GetAllUserWithLocation(double startLat, double startLng, string username)
        {
            //StoreLocation(username, startLat, startLng);
            string sql = "sp_locationhandler";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@username", username);
            parameters.Add("@flag", "i");
            parameters.Add("@latitude", startLat);
            parameters.Add("@longitude", startLng);
            var lst = await DbHelper.RunQueryWithModel<DistanceDetail>(sql, parameters, "y");
            return lst.FirstOrDefault();
        }

        public async Task<CommonResponse> ApproveUser(string username)
        {
            string sql = "sp_locationhandler";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            param.Add("@flag", "a");
            var resobj = await DbHelper.RunQueryWithModel<CommonResponse>(sql, param, "y");
            //throw new NotImplementedException();
            return resobj.FirstOrDefault();
        }
        public async Task<IEnumerable<string>> FriendList(string username)
        {
            string sql = "sp_locationhandler";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            param.Add("@flag", "l");
            IEnumerable<string> resobj = await DbHelper.RunQueryWithModel<string>(sql, param, "y");
            //throw new NotImplementedException();
            return resobj;
        }
        public CommonResponse Unfriend(string username, string targetusername)
        {
            string sql = "sp_locationhandler";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            param.Add("@targetusername", targetusername);
            param.Add("@flag", "u");
            var resobj = DbHelper.RunQueryWithModelWithoutasync<CommonResponse>(sql, param, "y");
            //throw new NotImplementedException();
            return resobj.FirstOrDefault();
        }



    }

}
