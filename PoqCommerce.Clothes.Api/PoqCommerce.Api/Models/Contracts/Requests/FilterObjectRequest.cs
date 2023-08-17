namespace PoqCommerce.Api.Models.Contracts.Requests
{
    public class FilterObjectRequest
    {
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? Size { get; set; }
        public string? Highlight { get; set; }
    }
}