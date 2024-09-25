using MessageChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace MessageChatApp.Hubs
{
	public class ChatHub : Hub
	{

		private readonly IServiceProvider _serviceProvider;

		public ChatHub(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task SendMessage(string idCon, string user, string message)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();

				var newMessage = new TbMessage
				{
					ConversationId = int.Parse(idCon),
					SenderId = int.Parse(user),
					Content = message,
					SentAt = DateTime.Now
				};

				dbContext.TbMessages.Add(newMessage);
				await dbContext.SaveChangesAsync();
			}

			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

	}


}
