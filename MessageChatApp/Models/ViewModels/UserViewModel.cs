namespace MessageChatApp.Models.ViewModels
{
    public class UserViewModel
    {
        public List<TbUser> Users { get; set; }
        public string CurrentUserName { get; set; }
		public List<TbUser> UsersWith { get; set; }

	}
}
