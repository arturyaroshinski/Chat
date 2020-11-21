using ChatApp.Models;
using ChatApp.Services;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IProfileService _profileService;

        public ProfileController(
            UserManager<User> userManager,
            IProfileService profileService
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
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

                await _profileService.UpdateAvatarAsync(model.File, user);

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
