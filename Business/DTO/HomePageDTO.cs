namespace Business.DTO
{
    public class HomePageDTO
    {
        public IEnumerable<int>? Sizes { get; set; }
        public IEnumerable<CategoryDTO>? Categories { get; set; }
        public IEnumerable<ProductDTO>? RecenntlyArrived { get; set; }
        public IEnumerable<ProductDTO>? HighlyRated { get; set; }
        public IEnumerable<BannerDTO>? Banners { get; set; }
    }
}
