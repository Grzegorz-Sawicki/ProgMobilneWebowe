using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.ConstrainedExecution;

namespace L1.Pages
{
    public class MessageModel : PageModel
    {
        public string Message { get; private set; } = "";

        private readonly string[] _messages = new string[] {
            "A champion is someone who gets up when he can't.",
            "If opportunity doesn't knock, build a door.",
            "The secret of getting ahead is getting started.",
            "When the going gets tough, the tough get going.",
            "The best way to predict the future is to create it.",
            "Winners never quit, and quitters never win.",
        };

        public void OnGet()
        {
            Random random = new Random();
            int index = random.Next(0, _messages.Length);
            Message = _messages[index];
        }
    }
}
