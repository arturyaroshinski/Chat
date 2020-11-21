using ChatApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class ProfileService : IProfileService
    {
        public async Task UpdateAvatarAsync(IFormFile file, User user)
        {
            if (file != null && file.Length > 0)
            {
                byte[] imageData = null;

                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    imageData = ms.ToArray();
                }

                user.Avatar = imageData;
            }
        }
    }
}
