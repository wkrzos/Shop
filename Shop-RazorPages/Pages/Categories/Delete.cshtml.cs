using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _db;

        public DeleteModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            Category = category;
            return Page();
        }

        public IActionResult OnPost()
        {
            var existing = _db.Categories.Find(Category.Id);
            if (existing == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(existing);
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
