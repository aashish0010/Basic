﻿using Basic.Application.Service;

namespace Basic.Domain.Interface
{
    public interface IUnitOfWork
    {
        public LoginRegisterService LoginRegisterService { get; }
        public DashBoardService dashBoardService { get; }
        public ForgetPasswordService ForgetPasswordService { get; }

        public EmailService emailService { get; }


    }
}
