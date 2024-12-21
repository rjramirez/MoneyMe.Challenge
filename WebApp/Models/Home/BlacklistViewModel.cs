namespace WebApp.Models.Home
{
    public class BlacklistViewModel
    {
        public int BlacklistId { get; set; }
        public string Domain { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
