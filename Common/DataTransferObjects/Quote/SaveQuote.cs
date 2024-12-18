using Common.DataTransferObjects._Base;
using System.ComponentModel.DataAnnotations;

namespace Common.DataTransferObjects.Quote
{
    public class SaveQuote : SaveDTOExtension
    {
        public decimal Amount { get; set; }
        [StringLength(4)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(20)]
        public string MobileNumber { get; set; }
    }
}
