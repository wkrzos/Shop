using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment

namespace Shop_RazorPages.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;  // Inject hosting environment

        public CreateModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public Article Article { get; set; } = new();

        // Property to handle uploaded file
        [BindProperty]
        public IFormFile? Upload { get; set; }

        public List<Category> Categories { get; set; } = new();

        public void OnGet()
        {
            // Load categories for dropdown
            Categories = _db.Categories.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload categories if form fails validation
                Categories = _db.Categories.ToList();
                return Page();
            }

            // Handle file upload if a file was submitted
            if (Upload != null)
            {
                // Generate unique file name + preserve extension
                var fileExt = Path.GetExtension(Upload.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExt}";

                // Physical path to wwwroot/uploads
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", fileName);

                // Save file to wwwroot/uploads
                using var fileStream = new FileStream(uploadPath, FileMode.Create);
                await Upload.CopyToAsync(fileStream);

                // Store relative path in the database (e.g. "/uploads/abc123.jpg")
                Article.ImagePath = $"/uploads/{fileName}";
            }

            _db.Articles.Add(Article);
            await _db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
