using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L6Backend.Models;
using L6Backend.DTO;
using System.Text.RegularExpressions;
using L6Backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace L6Backend.Controllers
{
    [Route("api/[controller]/{id}")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(DataContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages(int id)
        {
            var messages = await _context.Messages
                .Include(m => m.User)
                .Include(m => m.Chat)
                .Where(m => m.Chat.Id == id)
                .ToListAsync();

            var messageDTOs = messages.Select(m => new MessageDTO
            {
                Id = m.Id,
                Text = m.Text,
                Username = m.User.Name,
                PostDate = m.PostDate
            }).ToList();

            return messageDTOs;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(int id, CreateMessageDTO createMessageDTO)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == createMessageDTO.UserId);
            if (user == null)
            {
                return NotFound();
            }

            Message message = new Message
            {
                Text = createMessageDTO.Text,
                User = user,
                Chat = chat,
                PostDate = createMessageDTO.PostDate
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            var messageDTO = new MessageDTO
            {
                Text = message.Text,
                Id = message.Id,
                PostDate = message.PostDate,
                Username = message.User.Name
            };

            //await _hubContext.Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", messageDTO.Username, messageDTO.Text);

            return CreatedAtAction(nameof(PostMessage), new { id = message.Id }, messageDTO);
        }


    }
}