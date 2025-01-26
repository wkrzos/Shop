using Microsoft.AspNetCore.Mvc;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop_RazorPages.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ArticlesApiController(AppDbContext db)
        {
            _db = db;
        }

        // Example: GET /api/articles?skip=0&take=5
        [HttpGet]
        public async Task<ActionResult<List<Article>>> GetArticles(
            int skip = 0,
            int take = 5,
            int? categoryId = null)
        {
            IQueryable<Article> query = _db.Articles;

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            // The key is skip/take to fetch a subset of rows
            var articles = await query
                .OrderBy(a => a.Id) // ensure consistent order
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return articles;
        }
    }
}
