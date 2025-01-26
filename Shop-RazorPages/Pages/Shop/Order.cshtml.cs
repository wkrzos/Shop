using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Shop_RazorPages.Data;
using Shop_RazorPages.Models;

namespace Shop_RazorPages.Pages.Shop
{
    [Authorize] // Require login to access the Order page
    public class OrderModel : PageModel
    {
        private readonly AppDbContext _db;

        public List<CartItemViewModel> CartItems { get; set; } = new();

        // Payment methods for the dropdown
        public List<string> PaymentMethods { get; set; } = new() { "Credit Card", "PayPal", "Bank Transfer" };

        // Bind form fields
        [BindProperty]
        public OrderInputModel Input { get; set; } = new();

        public class OrderInputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [Display(Name = "Shipping Address")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "Payment Method")]
            public string PaymentMethod { get; set; }
        }

        public OrderModel(AppDbContext db)
        {
            _db = db;
        }

        // On GET: Show a read-only summary of cart items
        public async Task OnGetAsync()
        {
            CartItems = await LoadCartItems();
        }

        // On POST: User confirms order => redirect to final confirmation
        public async Task<IActionResult> OnPostAsync()
        {
            CartItems = await LoadCartItems();

            // Validate form
            if (!ModelState.IsValid)
            {
                return Page(); // re-display form with errors
            }

            // Save the user's form data in TempData or Session
            TempData["FullName"] = Input.FullName;
            TempData["Address"] = Input.Address;
            TempData["PaymentMethod"] = Input.PaymentMethod;

            // Next step (#6 in your instructions): go to a confirmation page
            return RedirectToPage("./OrderConfirmation");
        }

        // Helper: same logic used in CartModel to read cookies
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
    }
}
