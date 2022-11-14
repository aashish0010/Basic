using Basic.Infrastracture.Dapper;
using Dapper;

namespace Basic.Application.Function
{
    public static class CommonConfig
    {
        public static string ConfigValue(string configkey)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@configid", configkey);
            string sql = "select configvalue from Config where configid=@configid";
            var config = DbHelper.RunQueryDynamicallywithoutasync(sql, dynamicParameters);
            return config.FirstOrDefault().configvalue;
        }
    }
}
