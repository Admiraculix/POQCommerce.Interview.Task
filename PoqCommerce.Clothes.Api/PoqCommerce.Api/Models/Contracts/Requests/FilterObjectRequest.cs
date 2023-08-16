namespace PoqCommerce.Api.Models.Contracts.Requests
{
    public class FilterObjectRequest
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> CommonWords { get; set; }
    }
}