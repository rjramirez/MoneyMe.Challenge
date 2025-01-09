namespace Common.DataTransferObjects.Quote
{
    public class QuoteDetail
    {
        public int QuoteId { get; set; }
        //TODO: Make it an enum or a reference table
        public string Product { get; set; }
        public short Term { get; set; }
        public decimal AmountRequired { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal MonthlyRepaymentAmount { get; set; }
    }
}
