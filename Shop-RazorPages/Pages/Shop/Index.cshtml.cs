using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;   // adjust namespace
using Shop_RazorPages.Models; // adjust namespace

namespace Shop_RazorPages.Pages.Shop
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Category> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _db.Categories.ToListAsync();
        }
    }
}
