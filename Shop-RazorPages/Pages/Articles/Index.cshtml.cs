using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shop_RazorPages.Pages.Articles
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public List<Article> Articles { get; set; } = new();

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Articles = _db.Articles.Include(a => a.Category).ToList();
        }
    }
}
