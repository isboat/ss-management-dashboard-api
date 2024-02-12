using Management.Dashboard.Notification;
using Microsoft.Azure.SignalR.Management;

namespace Management.Dashboard.Api
{

    public interface IHubContextStore
    {
        public ServiceHubContext MessageHubContext { get; }
    }

    public class SignalRHostedService : IHostedService, IHubContextStore
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public ServiceHubContext MessageHubContext { get; private set; }

        public SignalRHostedService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            using var serviceManager = new ServiceManagerBuilder()
                .WithOptions(o=> {
                    o.ConnectionString = _configuration[NotificationConstants.AzureSignalRConnectionStringName];
                    o.ServiceTransportType = ServiceTransportType.Transient; 
                })
                .WithLoggerFactory(_loggerFactory)
                .BuildServiceManager();

            MessageHubContext = await serviceManager.CreateHubContextAsync(NotificationConstants.HubName, cancellationToken);
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(Dispose(MessageHubContext));
        }

        private static Task Dispose(ServiceHubContext hubContext)
        {
            if (hubContext == null)
            {
                return Task.CompletedTask;
            }
            return hubContext.DisposeAsync();
        }
    }
}
