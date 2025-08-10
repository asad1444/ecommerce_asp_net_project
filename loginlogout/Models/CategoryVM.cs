namespace loginlogout.Models
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public IFormFile CategoryPhoto { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; }

    }
}
