namespace termPaper.Models
{
    public class UpdateRequest
    {
        public string? SiteCode { get; set; }
        public string? RegisterNumber { get; set; }
        public string? Book { get; set; }
        public double Fee { get; set; }
    }
}