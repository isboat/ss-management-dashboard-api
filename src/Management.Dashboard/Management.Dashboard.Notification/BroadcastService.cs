namespace Management.Dashboard.Notification
{
    public class BroadcastService: IBroadcastService
    {
        private readonly IMessagePublisher _messagePublisher;

        public BroadcastService(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public async Task TryBroadcastAsync(ChangeMessage message)
        {
            await _messagePublisher.SendMessage(message);
        }
    }
}
