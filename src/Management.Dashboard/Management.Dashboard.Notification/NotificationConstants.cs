

namespace Management.Dashboard.Notification
{
    public class NotificationConstants
    {
        public const string ClientSideTargetEvent = "ReceiveChangeMessage";

        public const string AzureSignalRConnectionStringName = "AzureSignalRConnectionString";

        public const string HubName = "changelistenerhub";
    }

    public class NotificationExtensions
    {
        public static string ToSignalRUserId(string deviceId)
        {
            return $"device_id_{deviceId}";
        }

        public static string ToGroupName(string tenantId)
        {
            return $"grp-{tenantId}";
        }
    }
}
