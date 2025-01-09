namespace WebApp.Models.Home
{
    public class QuoteViewModel
    {
        public int QuoteId { get; set; }
        public string Product { get; set; }
        public decimal AmountRequired { get; set; }
        public short Term { get; set; }
        public decimal MonthlyRepaymentAmount { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public QuoteViewModel()
        {
            QuoteId = 0;
            AmountRequired = 0;
            Term = 0;
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
