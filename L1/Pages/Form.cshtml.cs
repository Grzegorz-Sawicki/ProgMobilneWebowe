using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace L1.Pages
{
    public class FormModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            ViewData["Username"] = Username;
            ViewData["Email"] = Email;
            ViewData["Message"] = Message;

            if (Username != null) HttpContext.Session.SetString("Username", Username);
            if (Email != null) HttpContext.Session.SetString("Email", Email);
            if (Message != null) HttpContext.Session.SetString("Message", Message);

            return Page();
        }
    }
}
