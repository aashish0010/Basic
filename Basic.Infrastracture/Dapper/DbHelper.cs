﻿using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Basic.Infrastracture.Dapper
{
    public static class DbHelper
    {
        public static SqlConnection Conn()
        {
            SqlConnection conn = new SqlConnection(DbConn.ConnectionString);
            conn.Open();
            return conn;

        }
        public static async Task<IEnumerable<T>> RunQueryWithModel<T>(string sql, object param = null, string isproc = null)
        {

            using (var con = Conn())
            {

                if (isproc == "y")
                {
                    var conn = await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
                    return conn;
                }
                else
                {
                    var conn = await con.QueryAsync<T>(sql, param);
                    return conn;
                }

            }

        }
        public static async Task<IEnumerable<dynamic>> RunQueryDynamically(string sql, object param = null, string isproc = null)
        {
            using (var con = Conn())
            {

                if (isproc == "y")
                {
                    var conn = await con.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return conn;
                }
                else
                {
                    var conn = await con.QueryAsync(sql, param);
                    return conn;
                }

            }
        }
        public static void Execute(string sql, string isproc = null, object param = null)
        {
            using (var con = Conn())
            {
                if (isproc == "y")
                {
                    con.Execute(sql, param, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    con.Execute(sql, param);
                }


            }
        }
        public static IEnumerable<dynamic> RunQueryDynamicallywithoutasync(string sql, object param = null, string isproc = null)
        {
            using (var con = Conn())
            {

                if (isproc == "y")
                {
                    var conn = con.Query(sql, param, commandType: CommandType.StoredProcedure);
                    return conn;
                }
                else
                {
                    var conn = con.Query(sql, param);
                    return conn;
                }

            }
        }
        public static IEnumerable<T> RunQueryWithModelWithoutasync<T>(string sql, object param = null, string isproc = null)
        {

            using (var con = Conn())
            {

                if (isproc == "y")
                {
                    var conn = con.Query<T>(sql, param, commandType: CommandType.StoredProcedure);
                    return conn;
                }
                else
                {
                    var conn = con.Query<T>(sql, param);
                    return conn;
                }

            }

        }

    }
}
