using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatApp.Database;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ChatDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            var chats = _dbContext.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            return View(chats);
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int Id)
        {
            var chat = _dbContext.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(c => c.Id == Id);

            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Public
            };

            chat.Users.Add(new ChatUser 
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Admin
            });

            _dbContext.Chats.Add(chat);

            // TODO: create repository for models
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.Member
            };

            //TODO: repo
            _dbContext.ChatUsers.Add(chatUser);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Chat", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string msg)
        {
            var message = new Message
            {
                ChatId = chatId,
                Content = msg,
                UserName = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            // TODO: repos
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId});
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
