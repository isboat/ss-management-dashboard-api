using System;
using System.Collections.Generic;
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

        public BroadcastService(
            string serviceBusConnectionString,
            string queueName,
            IMessagingBusService messagingBusService,
            IQueueClientFactory queueClientFactory)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _messagingBusService = messagingBusService;
            _queueClientFactory = queueClientFactory;
            _queueName = queueName;
        }

        public async Task TryBroadcastAsync(ChangeMessage message)
        {
            var client = _queueClientFactory.CreateSender(_serviceBusConnectionString, _queueName);
            await _messagingBusService.SendMessageAsync(message, client);
        }
    }
}
