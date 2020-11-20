using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatApp.Database;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Repository;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Hubs;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatRepository _chatRepository;

        public HomeController(ILogger<HomeController> logger, IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            var chats = _chatRepository.GetChatsWithoutUser(GetUserId());

            return View(chats);
        }

        public IActionResult Find([FromServices] ChatDbContext dbContext)
        {
            // TODO: refactor
            var userChats = _chatRepository.GetPrivateChats(GetUserId());

            var users = dbContext.Users
                .Where(x => x.Id != GetUserId())
                .ToList();
            
            foreach (var chat in userChats)
            {
                var chatUsers = chat.Users.Where(x => x.UserId != GetUserId());
                foreach (var chatuser in chatUsers)
                {
                    if (chat.Users.Contains(chatuser))
                    {
                        users.Remove(chatuser.User);
                    }
                }
            }

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = _chatRepository.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var id = await _chatRepository.CreatePrivateRoom(GetUserId(), userId);

            return id == -1 ? RedirectToAction("Private") : RedirectToAction("Chat", new { id });
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            return View(_chatRepository.GetChat(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _chatRepository.CreateRoom(name, GetUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _chatRepository.JoinRoom(GetUserId(), id);
            
            return RedirectToAction("Chat", new { id });
        }

        public async Task<IActionResult> SendMessage(
            int roomId,
            string msg,
            [FromServices] IHubContext<ChatHub> chat)
        {
            var message = await _chatRepository.CreateMessage(roomId, User.Identity.Name, msg);

            await chat.Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", new
            {
                message.Content,
                message.UserName,
                Timestamp = message.Timestamp.ToString("dd/mm/yyyy hh:mm:ss")
            });

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
