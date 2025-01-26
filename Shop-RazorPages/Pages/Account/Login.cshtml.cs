using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop_RazorPages.Pages.Account
{
    public class LoginModel(SignInManager<IdentityUser> signInManager) : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;

        [BindProperty]
        public required InputModel Input { get; set; }

        public required string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public required string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public required string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Content("~/"); // Redirect to home if no returnUrl
            }

            var result = await _signInManager.PasswordSignInAsync(
                Input.Email, 
                Input.Password, 
                Input.RememberMe, 
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
