﻿using System;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Win10.Services
{
    internal class PasswordService : IPasswordService
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public void LoadCredential()
        {
            throw new NotImplementedException();
        }

        public void SaveCredential()
        {
            throw new NotImplementedException();
        }

        public void DeleteCredential()
        {
            throw new NotImplementedException();
        }
    }
}
