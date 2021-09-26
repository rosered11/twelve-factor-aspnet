using System;
using System.Globalization;
using account.dataaccess.entities;
using account.dto.requests;
using Rosered11.Aspnet.Utilities;

namespace account.dataaccess.repositories
{
    public interface IAccountRepository
    {
        Account FindByCustomerId(Customer customer);
    }

    public class MockAccountRepository : IAccountRepository
    {
        public Account FindByCustomerId(Customer customer)
        {
            var utcnow = DateTime.UtcNow;
            return new Account { 
                CustomerId = "1"
                , AccountNumber = "1"
                , AccountType = "Savings"
                , BranchAddress = "123 main"
                // Location of time zone follow host os.
                // Asia/Bangkok setting follow mac os
                , CreateDate = DateTimeUtility.ChangeTimeZone(utcnow, "Asia/Bangkok").ToString("yyyy-MM-ddTHH:mm:sszzz")
            };
        }
    }
}