using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Display(Name = "Avatar")]
        public byte[] Avatar { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public IFormFile File { get; set; }
    }
}
