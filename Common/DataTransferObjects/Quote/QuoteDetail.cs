using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferObjects.Quote
{
    public class QuoteDetail
    {
        public int QuoteId { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
