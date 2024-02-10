
using Azure.Messaging.ServiceBus;

namespace Management.Dashboard.Notification
{
    public interface IQueueClientFactory
    {
        ServiceBusSender CreateSender(string connectionString, string queueName);

    }
}