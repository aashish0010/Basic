﻿using Basic.Domain.Entity;
using Basic.Domain.Interface;
using Basic.Infrastracture.Dapper;
using Dapper;

namespace Basic.Application.Service
{
    public class LoginRegisterService : ILoginRegisterService
    {
        public async Task<IEnumerable<Uservalidate>> Login(LoginRequest login)
        {
            var uval = new Uservalidate();
            var sql = "sp_user";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", login.UserName);
            param.Add("@password", login.Password);
            param.Add("@flag", "Login");
            var loginval = await DbHelper.RunQueryWithModel<Uservalidate>(sql, "y", param);
            return loginval;

        }

        public async Task<IEnumerable<CommonResponse>> Register(Register reg)
        {
            var uval = new Uservalidate();
            var sql = "sp_user";
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", reg.Username);
            param.Add("@password", reg.Password);
            param.Add("@email", reg.Email);
            param.Add("@firstname", reg.FirstName);
            param.Add("@middlename", reg.MiddleName);
            param.Add("@lastname", reg.LastName);
            param.Add("@phonenum", reg.PhoneNumber);
            param.Add("@flag", "Register");
            var regval = await DbHelper.RunQueryWithModel<CommonResponse>(sql, "y", param);
            return regval;
        }

    }
}