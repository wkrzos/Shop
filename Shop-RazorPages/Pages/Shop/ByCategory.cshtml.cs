using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Shop
{
    public class ByCategoryModel : PageModel
    {
        private readonly AppDbContext _db;

        public ByCategoryModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string SelectedCategoryName { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var category = await _db.Categories
                .Include(c => c.Articles)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                return NotFound();
            }

            SelectedCategoryName = category.Name;
            Articles = category.Articles.ToList();
            return Page();
        }
    }
}
