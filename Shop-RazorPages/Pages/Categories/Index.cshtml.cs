using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Category> Categories { get; set; } = new();

        public void OnGet()
        {
            // Eager-load the Articles if you want to display or count them
            Categories = _db.Categories
                            .Include(c => c.Articles)
                            .ToList();
        }
    }
}
