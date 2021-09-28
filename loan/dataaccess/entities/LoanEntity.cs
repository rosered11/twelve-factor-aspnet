namespace loan.dataaccess.entities
{
    public class LoanEntity
    {
        public string CustomerId { get; set; }
        public string StartDate { get; set; }
        public string LoanType { get; set; }
        public decimal TotalLoan { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal OutStandingAmount { get; set; }
        public string CreateDate { get; set; }
    }
}