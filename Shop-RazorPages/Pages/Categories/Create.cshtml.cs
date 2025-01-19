using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;

        public CreateModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public void OnGet()
        {
            // Nothing special to do on GET, unless you want to load other data
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Show form again
            }

            _db.Categories.Add(Category);
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
