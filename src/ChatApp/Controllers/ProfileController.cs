using ChatApp.Models;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new ProfileViewModel()
            {
                UserName = user.UserName,
                Avatar = user.Avatar,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (model.File != null && model.File.Length > 0)
                {
                    byte[] imageData = null;
                    using (var br = new BinaryReader(model.File.OpenReadStream()))
                    {
                        imageData = br.ReadBytes((int)model.File.Length);
                    }

                    user.Avatar = imageData;
                }
                // TODO: profileService for editing it;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }

        public async Task<FileResult> GetImage()
        {
            var user = await _userManager.GetUserAsync(User);

            return File(user.Avatar, "image/jpeg");
        }
    }
}
