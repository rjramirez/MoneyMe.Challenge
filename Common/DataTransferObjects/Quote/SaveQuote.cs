using Common.DataTransferObjects._Base;

namespace Common.DataTransferObjects.Quote
{
    public class SaveQuote : SaveDTOExtension
    {
        public float Amount { get; set; }

        public string Salutation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EMail { get; set; }

        public string MobileNumber { get; set; }
    }
}
