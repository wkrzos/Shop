using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Articles
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;

        public EditModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        public List<Category> Categories { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var article = _db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            Article = article;
            Categories = _db.Categories.ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Categories = _db.Categories.ToList();
                return Page();
            }

            _db.Articles.Update(Article);
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
