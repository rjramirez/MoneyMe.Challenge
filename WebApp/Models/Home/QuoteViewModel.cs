namespace WebApp.Models.Home
{
    public class QuoteViewModel
    {
        public int QuoteId { get; set; }
        public decimal Amount { get; set; }
        public string Term { get; set; }
        public decimal MonthlyRepaymentAmount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public QuoteViewModel()
        {
            QuoteId = 0;
            Amount = 0;
            Term = string.Empty;
            MonthlyRepaymentAmount = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            CreatedDate = DateTime.MinValue;
            UpdatedDate = DateTime.MinValue;
        }
    }
}
