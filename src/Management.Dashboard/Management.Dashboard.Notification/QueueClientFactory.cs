using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;


namespace Management.Dashboard.Notification
{
    public class QueueClientFactory : IQueueClientFactory
    {
        public ServiceBusSender CreateSender(string connectionString, string topicName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
            // If you use the default AmqpTcp, ensure that ports 5671 and 5672 are open.
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            try
            {
                var client = new ServiceBusClient(
                    connectionString,
                    clientOptions);

                var sender = client.CreateSender(topicName);

                return sender;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}