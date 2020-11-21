using ChatApp.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IProfileService
    {
        Task UpdateAvatarAsync(IFormFile file, User user);
    }
}
