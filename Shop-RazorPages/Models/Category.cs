// Models/Category.cs
namespace Shop_RazorPages.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
