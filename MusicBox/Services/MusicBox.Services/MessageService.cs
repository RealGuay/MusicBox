using MusicBox.Services.Interfaces;

namespace MusicBox.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
