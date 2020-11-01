using ChatApp.Database;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _dbContext;

        public ChatRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new System.ArgumentNullException(nameof(dbContext));
        }

        public Chat GetChat(int Id)
        {
            return _dbContext.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(c => c.Id == Id);
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Public
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _dbContext.Chats.Add(chat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task JoinRoom(string userId, int chatId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _dbContext.ChatUsers.Add(chatUser);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            //TODO: check for existing
            var chat = new Chat
            {
                Type = ChatType.Private,
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId
            });

            _dbContext.Chats.Add(chat);
            await _dbContext.SaveChangesAsync();

            return chat.Id;
        }
        public IEnumerable<Chat> GetChats(string userId)
        {
            return _dbContext.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _dbContext.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private
                    && x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public async Task<Message> CreateMessage(int chatId, string userName, string msg)
        {
            var message = new Message
            {
                ChatId = chatId,
                Content = msg,
                UserName = userName,
                Timestamp = DateTime.Now
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            return message;
        }
    }
}
