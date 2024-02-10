using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace Management.Dashboard.Notification
{
    public interface IMessagingBusService
    {
        /// <summary>
        /// Send message to topic client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sender"></param>
        /// <param name="messageSessionId"></param>
        /// <returns></returns>
        Task SendMessageAsync<T>(T data, ServiceBusSender sender);
    }
}