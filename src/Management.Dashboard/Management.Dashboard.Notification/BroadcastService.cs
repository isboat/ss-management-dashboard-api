using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Notification
{
    public class BroadcastService: IBroadcastService
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _queueName;
        private readonly IQueueClientFactory _queueClientFactory;
        private readonly IMessagingBusService _messagingBusService;
        private readonly IMessagePublisher _messagePublisher;

        public BroadcastService(
            string serviceBusConnectionString,
            string queueName,
            IMessagingBusService messagingBusService,
            IQueueClientFactory queueClientFactory,
            IMessagePublisher messagePublisher)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _messagingBusService = messagingBusService;
            _queueClientFactory = queueClientFactory;
            _queueName = queueName;
            _messagePublisher = messagePublisher;
        }

        public async Task TryBroadcastAsync(ChangeMessage message)
        {
            await _messagePublisher.SendMessage(message);

            //var client = _queueClientFactory.CreateSender(_serviceBusConnectionString, _queueName);
            //await _messagingBusService.SendMessageAsync(message, client);
        }
    }
}
