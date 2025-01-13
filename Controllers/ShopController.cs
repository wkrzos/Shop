using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ShopController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShopController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Shop
    public async Task<IActionResult> Index()
    {
        ViewData["Categories"] = await _context.Categories.ToListAsync();
        return View();
    }

    // GET: Shop/ByCategory/{id}
    public async Task<IActionResult> ByCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Articles)
            .ThenInclude(a => a.Category)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        ViewData["SelectedCategory"] = category.Name;
        return View(category.Articles);
    }
}
