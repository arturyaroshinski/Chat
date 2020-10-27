using ChatApp.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ChatApp.VewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly ChatDbContext _dbContext;

        public RoomViewComponent(ChatDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public IViewComponentResult Invoke()
        {
            // TODO: repository
            var chats = _dbContext.Chats.ToList();

            return View(chats);
        }
    }
}
