namespace Common.DataTransferObjects.Quote
{
    public class BlacklistDetail
    {
        public int BlacklistId { get; set; }
        public string Domain { get; set; }
        public string Mobile { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
