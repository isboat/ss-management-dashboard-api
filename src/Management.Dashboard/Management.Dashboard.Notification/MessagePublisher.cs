using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Management.Dashboard.Notification
{
    public class MessagePublisher: IMessagePublisher
    {
        private readonly string _connectionString;
        private readonly ServiceTransportType _serviceTransportType;
        private ServiceHubContext _hubContext;
        private readonly ILogger<MessagePublisher> _logger;

        public MessagePublisher(string connectionString, ServiceTransportType serviceTransportType, ILogger<MessagePublisher> logger)
        {
            _connectionString = connectionString;
            _serviceTransportType = serviceTransportType;
            _logger = logger;

            _ = InitAsync();
        }

        private async Task InitAsync()
        {
            var serviceManager = new ServiceManagerBuilder().WithOptions(option =>
            {
                option.ConnectionString = _connectionString;
                option.ServiceTransportType = _serviceTransportType;
            })
            //Uncomment the following line to get more logs
            //.WithLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .BuildServiceManager();

            _hubContext = await serviceManager.CreateHubContextAsync(NotificationConstants.HubName, default);
        }

        public Task SendMessages(string command, string receiver, string message)
        {
            try
            {
                switch (command)
                {
                    case "broadcast":
                        return _hubContext.Clients.All.SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                    case "user":
                        var userId = receiver;
                        return _hubContext.Clients.User(userId).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                    case "users":
                        var userIds = receiver.Split(',');
                        return _hubContext.Clients.Users(userIds).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                    case "group":
                        var groupName = receiver;
                        return _hubContext.Clients.Group(groupName).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                    case "groups":
                        var groupNames = receiver.Split(',');
                        return _hubContext.Clients.Groups(groupNames).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                    default:
                        Console.WriteLine($"Can't recognize command {command}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
            }
            return Task.CompletedTask;
        }

        public Task SendMessage(ChangeMessage changeMessage)
        {
            try
            {
                var message = JsonConvert.SerializeObject(changeMessage, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                _logger.LogWarning(message);

                if (!string.IsNullOrEmpty(changeMessage.DeviceId))
                {
                    var userId = NotificationExtensions.ToSignalRUserId(changeMessage.DeviceId);
                    _hubContext.Clients.User(userId).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                }

                if (!string.IsNullOrEmpty(changeMessage.TenantId))
                {
                    var grpName = NotificationExtensions.ToGroupName(changeMessage.TenantId);
                    _hubContext.Clients.Group(grpName).SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                }

                if (string.IsNullOrEmpty(changeMessage.TenantId) && string.IsNullOrEmpty(changeMessage.DeviceId))
                {
                    _hubContext.Clients.All.SendAsync(NotificationConstants.ClientSideTargetEvent, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
            }

            return Task.CompletedTask;
        }

        public Task CloseConnection(string connectionId, string reason)
        {
            return _hubContext.ClientManager.CloseConnectionAsync(connectionId, reason);
        }

        public Task<bool> CheckExist(string type, string id)
        {
            return type switch
            {
                "connection" => _hubContext.ClientManager.ConnectionExistsAsync(id),
                "user" => _hubContext.ClientManager.UserExistsAsync(id),
                "group" => _hubContext.ClientManager.UserExistsAsync(id),
                _ => throw new NotSupportedException(),
            };
        }

        public Task DisposeAsync() => _hubContext?.DisposeAsync();
    }
}
