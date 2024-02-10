using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Management.Dashboard.Notification
{
    public class MessagingBusService: IMessagingBusService
    {
        public async Task SendMessageAsync<T>(T data, ServiceBusSender sender)
        {
            var messageBody = JsonConvert.SerializeObject(data,
                new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            messageBatch.TryAddMessage(new ServiceBusMessage(messageBody));

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
            }
        }
    }
}