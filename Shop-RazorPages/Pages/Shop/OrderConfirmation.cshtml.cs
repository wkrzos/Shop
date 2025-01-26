using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Shop
{
    [Authorize] // The user must be logged in
    public class OrderConfirmationModel : PageModel
    {
        private readonly AppDbContext _db;

        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }

        public List<CartItemViewModel> CartItems { get; set; } = new();

        public OrderConfirmationModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            // 1. Retrieve order details from TempData (saved in Order.cshtml.cs OnPostAsync)
            FullName = TempData["FullName"] as string;
            Address = TempData["Address"] as string;
            PaymentMethod = TempData["PaymentMethod"] as string;

            // 2. Load the cart items (from cookies, same method as Cart/Order pages)
            CartItems = await LoadCartItems();

            // (Optional) 3. Save the order to the database 
            //     If you want to store in DB, you’d do it here (or in the POST).
            //     For example:
            //         var order = new Order { FullName = FullName, ... };
            //         _db.Orders.Add(order);
            //         await _db.SaveChangesAsync();
            //         Then save each CartItem to OrderItems table, etc.

            // 4. Clear the cart cookies
            ClearCartCookies();
        }

        private async Task<List<CartItemViewModel>> LoadCartItems()
        {
            var items = new List<CartItemViewModel>();

            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith("article"))
                {
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
            return items;
        }

        private void ClearCartCookies()
        {
            foreach (var cookie in Request.Cookies)
            {
                if (cookie.Key.StartsWith("article"))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }
    }
}
