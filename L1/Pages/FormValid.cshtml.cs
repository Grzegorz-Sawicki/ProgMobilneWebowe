using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace L1.Pages
{
    public class FormValidModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password at least 6 characters long!")]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Confirm Password required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
			if (!ModelState.IsValid)
			{
				return Page();
			}

            return RedirectToPage("Success");
		}
    }
}
