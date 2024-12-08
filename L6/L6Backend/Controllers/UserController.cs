using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L6Backend.Models;
using L6Backend.DTO;

namespace L6Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.ChatGroups)
                .Include(u => u.Messages)
                .ToListAsync();

            var userDTOs = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                ChatGroupIds = u.ChatGroups.Select(cg => cg.Id).ToList()
            }).ToList();

            return userDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.ChatGroups)
                .Include(u => u.Messages)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                ChatGroupIds = user.ChatGroups.Select(cg => cg.Id).ToList()
            };

            return userDTO;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(CreateUserDTO createUserDTO)
        {
            var existingUser = await _context.Users
                .Include(u => u.ChatGroups)
                .FirstOrDefaultAsync(u => u.Name == createUserDTO.Name);

            if (existingUser != null)
            {
                var existingUserDTO = new UserDTO
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    ChatGroupIds = existingUser.ChatGroups.Select(cg => cg.Id).ToList()
                };

                return Ok(existingUserDTO);
            }

            User user = new User
            {
                Name = createUserDTO.Name
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var newUserDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                ChatGroupIds = new List<int>()
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, newUserDTO);
        }


    }
}