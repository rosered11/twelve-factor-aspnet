using System;
using System.Collections.Generic;
using loan.dataaccess.entities;
using loan.dto.requests;
using Rosered11.Aspnet.Utilities;

namespace loan.dataaccess.repositories
{
    public interface ILoanRepository
    {
        IEnumerable<LoanEntity> FindByCustomerId(Customer customer);
    }
    public class MockLoanRepository : ILoanRepository
    {
        public IEnumerable<LoanEntity> FindByCustomerId(Customer customer)
        {
            var utcNow = DateTime.UtcNow;
            var data = new List<LoanEntity>();

            data.Add(
                new LoanEntity { 
                    CustomerId = "1"
                    , StartDate = DateTimeUtility.ChangeTimeZone(utcNow, "Asia/Bangkok").ToString("yyyy-MM-ddTHH:mm:sszzz")
                    , LoanType = "Home"
                    , TotalLoan = 1234.11M
                    , AmountPaid = 2345.22M
                    , OutStandingAmount = 4567.33M
                    , CreateDate = DateTimeUtility.ChangeTimeZone(utcNow, "Asia/Bangkok").ToString("yyyy-MM-ddTHH:mm:sszzz")
                }
            );
            return data;
        }
    }
}