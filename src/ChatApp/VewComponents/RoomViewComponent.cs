using ChatApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace ChatApp.VewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly IChatRepository _chatRepository;

        public RoomViewComponent(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }
        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _chatRepository.GetChatsWithUser(userId);

            return View(chats);
        }
    }
}
