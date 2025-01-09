using Common.DataTransferObjects._Base;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferObjects.Quote
{
    public class SaveQuote : SaveDTOExtension
    {
        public int QuoteId { get; set; }
        public string Product { get; set; }
        public decimal AmountRequired { get; set; }
        public short Term { get; set; }
        [StringLength(4)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
    }
}
