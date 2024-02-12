namespace Management.Dashboard.Notification
{
    public interface IMessagePublisher
    {
        Task ManageUserGroup(string command, string userId, string groupName);
        Task SendMessages(string command, string receiver, string message);
        Task SendMessage(ChangeMessage changeMessage);
    }
}
