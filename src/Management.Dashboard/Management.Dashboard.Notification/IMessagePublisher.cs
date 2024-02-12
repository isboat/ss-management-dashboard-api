namespace Management.Dashboard.Notification
{
    public interface IMessagePublisher
    {
        Task SendMessages(string command, string receiver, string message);
        Task SendMessage(ChangeMessage changeMessage);
    }
}
