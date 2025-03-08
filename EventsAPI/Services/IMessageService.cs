namespace EventsAPI.Services
{
    public interface IMessageService
    {
        public Task SendAsync(string email, string subject, string message);
    }
}
