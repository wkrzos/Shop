namespace Shop_RazorPages.Models
{
    public class CartItemViewModel
    {
        public int ArticleId { get; set; }
        public string ArticleName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}
