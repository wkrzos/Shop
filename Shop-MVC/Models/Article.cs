public class Article
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public string? ImagePath { get; set; }

    public DateTime ExpiryDate { get; set; }
    
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
