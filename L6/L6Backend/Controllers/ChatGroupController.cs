using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L6Backend.Models;
using L6Backend.DTO;
using L6Backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace L6Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGroupController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatGroupController(DataContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatGroupDTO>>> GetChatGroups()
        {
            var chatGroups = await _context.ChatGroups
                .Include(cg => cg.Users)
                .Include(cg => cg.Chat)
                .ThenInclude(chat => chat.Messages)
                .ThenInclude(message => message.User)
                .ToListAsync();


            var chatGroupDTOs = chatGroups.Select(cg => new ChatGroupDTO
            {
                Id = cg.Id,
                Name = cg.Name,
                Chat = new ChatDTO
                {
                    Id = cg.Chat.Id,
                    Messages = cg.Chat.Messages != null ? cg.Chat.Messages.Select(message => new MessageDTO
                    {
                        Id = message.Id,
                        Text = message.Text,
                        Username = message.User.Name,
                        PostDate = message.PostDate
                    }).ToList() : new List<MessageDTO>()
                },
                Users = cg.Users.Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name

                }).ToList()
            }).ToList();

            return chatGroupDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatGroupDTO>> GetChatGroup(int id)
        {
            var chatGroup = await _context.ChatGroups
                .Include(cg => cg.Users)
                .Include(cg => cg.Chat)
                .ThenInclude(chat => chat.Messages)
                .ThenInclude(message => message.User)
                .FirstOrDefaultAsync(cg => cg.Id == id);

            if (chatGroup == null)
            {
                return NotFound();
            }

            Console.WriteLine(chatGroup.Users);

            var chatGroupDTO = new ChatGroupDTO
            {
                Id = chatGroup.Id,
                Name = chatGroup.Name,
                Chat = new ChatDTO
                {
                    Id = chatGroup.Chat.Id,
                    Messages = chatGroup.Chat.Messages != null ? chatGroup.Chat.Messages.Select(message => new MessageDTO
                    {
                        Id = message.Id,
                        Text = message.Text,
                        Username = message.User.Name,
                        PostDate = message.PostDate
                    }).ToList() : new List<MessageDTO>()
                },
                Users = chatGroup.Users.Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name

                }).ToList()
            };

            return chatGroupDTO;
        }

        [HttpPost]
        public async Task<ActionResult<ChatGroup>> PostChatGroup(CreateChatGroupDTO createChatGroupDTO)
        {
            ChatGroup chatGroup = new ChatGroup
            {
                Name = createChatGroupDTO.Name
            };

            _context.ChatGroups.Add(chatGroup);
            await _context.SaveChangesAsync();

            var chatGroupDTO = new ChatGroupDTO
            {
                Id = chatGroup.Id,
                Name = chatGroup.Name,
                Chat = new ChatDTO
                {
                    Id = chatGroup.Chat.Id,
                    Messages = new List<MessageDTO>()
                },
                Users = new List<UserDTO>()
            };

            return CreatedAtAction(nameof(GetChatGroup), new { id = chatGroup.Id }, chatGroupDTO);
        }

        [HttpPost("{groupId}/users/{userId}")]
        public async Task<IActionResult> AddUserToGroup(int groupId, int userId)
        {
            var chatGroup = await _context.ChatGroups
                .Include(cg => cg.Users)
                .FirstOrDefaultAsync(cg => cg.Id == groupId);

            if (chatGroup == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            chatGroup.Users.Add(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{groupId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
        {
            var chatGroup = await _context.ChatGroups
                .Include(cg => cg.Users)
                .FirstOrDefaultAsync(cg => cg.Id == groupId);

            if (chatGroup == null)
            {
                return NotFound();
            }

            var user = chatGroup.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            chatGroup.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}