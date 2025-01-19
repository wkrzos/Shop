using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Shop
{
    public class CartModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        // We'll store the cart items here for display
        public List<CartItemViewModel> CartItems { get; set; } = new();

        public CartModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // 1. Display the cart
        public async Task OnGetAsync()
        {
            await LoadCartItems();
        }

        // 2. Add to Cart
        //   /Shop/Cart?handler=AddToCart&articleId=XYZ
        public async Task<IActionResult> OnGetAddToCart(int articleId)
        {
            string cookieKey = $"article{articleId}";

            // Check if exists
            if (Request.Cookies.TryGetValue(cookieKey, out var currentValue))
            {
                if (int.TryParse(currentValue, out int quantity))
                {
                    quantity++;
                    SetCartCookie(cookieKey, quantity);
                }
            }
            else
            {
                // Not in cart -> quantity=1
                SetCartCookie(cookieKey, 1);
            }

            return RedirectToPage(); // Refresh cart page
        }

        // 3. Remove One
        //   /Shop/Cart?handler=RemoveOne&articleId=XYZ
        public async Task<IActionResult> OnGetRemoveOne(int articleId)
        {
            string cookieKey = $"article{articleId}";

            if (Request.Cookies.TryGetValue(cookieKey, out var currentValue))
            {
                if (int.TryParse(currentValue, out int quantity))
                {
                    if (quantity > 1)
                    {
                        quantity--;
                        SetCartCookie(cookieKey, quantity);
                    }
                    else
                    {
                        // quantity=1 => remove the cookie altogether
                        Response.Cookies.Delete(cookieKey);
                    }
                }
            }

            return RedirectToPage();
        }

        // 4. Remove Entire Item
        //   /Shop/Cart?handler=RemoveItem&articleId=XYZ
        public async Task<IActionResult> OnGetRemoveItem(int articleId)
        {
            string cookieKey = $"article{articleId}";
            Response.Cookies.Delete(cookieKey);
            return RedirectToPage();
        }

        // (Optional) Clear entire cart
        public IActionResult OnGetClearCart()
        {
            // remove all article cookies
            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith("article"))
                    Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToPage();
        }

        // Helper: load cart items from cookies
        private async Task LoadCartItems()
        {
            var items = new List<CartItemViewModel>();

            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith("article"))
                {
                    // Extract article ID
                    var articleIdStr = cookie.Key.Substring("article".Length);
                    if (int.TryParse(articleIdStr, out int articleId))
                    {
                        if (int.TryParse(cookie.Value, out int quantity))
                        {
                            var article = await _db.Articles.FindAsync(articleId);
                            if (article != null)
                            {
                                items.Add(new CartItemViewModel
                                {
                                    ArticleId = article.Id,
                                    ArticleName = article.Name,
                                    Quantity = quantity,
                                    Price = article.Price
                                });
                            }
                        }
                    }
                }
            }

            CartItems = items;
        }

        // Helper: set cookie with 7-day expiration
        private void SetCartCookie(string key, int quantity)
        {
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                Path = "/"
            };
            Response.Cookies.Append(key, quantity.ToString(), options);
        }
    }
}
