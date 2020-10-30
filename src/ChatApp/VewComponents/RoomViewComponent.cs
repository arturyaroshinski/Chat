﻿using ChatApp.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

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
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _dbContext.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == userId)
                .Select(x => x.Chat)
                .ToList();

            return View(chats);
        }
    }
}
