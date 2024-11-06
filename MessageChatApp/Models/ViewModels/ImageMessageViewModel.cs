
using System.ComponentModel.DataAnnotations;

namespace MessageChatApp.Models.ViewModels
{
    public class ImageMessageViewModel
    {
        [Display(Name = "Upload Image")]
        public IFormFile ImageUpload { get; set; }
    }
}
