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

        public async Task SendMessage(string idCon, string senderId, string message, string imageMessage)
        {
            try
            {
                if (imageMessage != null)
                {
                    Console.WriteLine("image: " + imageMessage);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    //string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageMessage.FileName;
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();

                    var newMessage = new TbMessage
                    {
                        ConversationId = int.Parse(idCon),
                        SenderId = int.Parse(senderId),
                        Content = message,
                        SentAt = DateTime.Now,
                    };

                    dbContext.TbMessages.Add(newMessage);
                    await dbContext.SaveChangesAsync();
                }

                await Clients.All.SendAsync("ReceiveMessage", senderId, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }


}
