using ChatApp.Database;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat ?? throw new ArgumentNullException(nameof(chat));
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            string msg,
            string roomName,
            [FromServices] ChatDbContext dbContext)
        {
            var message = new Message
            {
                ChatId  = chatId,
                Content = msg,
                UserName = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            dbContext.Messages.Add(message);
            await dbContext.SaveChangesAsync();

            await _chat.Clients.Group(roomName).SendAsync("RecieveMessage", new 
            {
                message.Content,
                message.UserName,
                Timestamp = message.Timestamp.ToString("dd/mm/yyyy hh:mm:ss")
            });

            return Ok();
        }
    }
}
