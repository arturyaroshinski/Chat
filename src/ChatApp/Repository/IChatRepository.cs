using ChatApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Repository
{
    public interface IChatRepository
    {
        Chat GetChat(int Id);

        Task CreateRoom(string name, string userId);

        Task JoinRoom(string userId, int chatId);

        IEnumerable<Chat> GetChatsWithoutUser(string userId);
        IEnumerable<Chat> GetChatsWithUser(string userId);

        Task<int> CreatePrivateRoom(string rootId, string targetId);

        IEnumerable<Chat> GetPrivateChats(string userId);

        Task<Message> CreateMessage(int chatId, string userId, string msg);
    }
}
