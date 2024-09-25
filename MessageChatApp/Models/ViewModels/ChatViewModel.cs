namespace MessageChatApp.Models.ViewModels
{
    public class ChatViewModel
    {
        public TbConversation Conversation { get; set; }
        public List<TbMessage> Messages { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
		public int CurrentUserId { get; set; }
	}
}
