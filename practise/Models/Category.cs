namespace practise.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public ICollection <Product> Product { get; set; }
    }
}
