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
        private const int PageSize = 5; // how many articles to show initially

        public ByCategoryModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public string SelectedCategoryName { get; set; } = string.Empty;

        // The initial chunk of articles to display
        public List<Article> Articles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Load the category
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
                return NotFound();

            SelectedCategoryName = category.Name;

            // Load only the first chunk of articles for this category
            Articles = await _db.Articles
                .Where(a => a.CategoryId == Id)
                .OrderBy(a => a.Id)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }
    }
}
