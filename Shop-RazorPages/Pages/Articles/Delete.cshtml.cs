using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Shop_RazorPages.Pages.Articles
{
    [Authorize(Policy = "AdminPolicy")]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var article = _db.Articles.Find(id);
            if (article == null)
            {
                return NotFound();
            }

            Article = article;
            return Page();
        }

        public IActionResult OnPost()
        {
            var existing = _db.Articles.Find(Article.Id);
            if (existing == null)
            {
                return NotFound();
            }

            // Remove the photo from disk if it exists
            if (!string.IsNullOrEmpty(existing.ImagePath))
            {
                // e.g. "/uploads/abc123.jpg" -> "abc123.jpg"
                var fileName = Path.GetFileName(existing.ImagePath);
                var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _db.Articles.Remove(existing);
            _db.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
