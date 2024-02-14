using Management.Dashboard.Common;
using Management.Dashboard.Models;
using Management.Dashboard.Notification;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services;
using Management.Dashboard.Services.Interfaces;
using Microsoft.Azure.SignalR.Management;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Management.Dashboard.Api
{
    public static class ServiceRegisterExtension
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRepository<ScreenModel>, ScreenRepository>();
            builder.Services.AddSingleton<IRepository<MenuModel>, MenuRepository>();
            builder.Services.AddSingleton<ITemplatesRepository, TemplatesRepository>();
            builder.Services.AddSingleton<IUserRepository<UserModel>, UserRepository>();
            builder.Services.AddSingleton<IDeviceAuthRepository<DeviceAuthModel>, DeviceAuthRepository>();
            builder.Services.AddSingleton<IRepository<AssetItemModel>, AssetRepository>();
            builder.Services.AddSingleton<IRepository<TextAssetItemModel>, TextAssetRepository>();
            builder.Services.AddSingleton<IRepository<DeviceModel>, DeviceRepository>();
            builder.Services.AddSingleton<IRepository<PlaylistModel>, PlaylistsRepository>();
            builder.Services.AddSingleton<ITenantRepository, TenantRepository>();
            builder.Services.AddSingleton<IPublishRepository, PublishRepository>();
            builder.Services.AddSingleton<IHistoryRepository, HistoryRepository>();

            var objectSerializer = new ObjectSerializer(
                type => ObjectSerializer.DefaultAllowedTypes(type)
                || (type?.FullName != null && type.FullName.StartsWith("Management.Dashboard")));

            BsonSerializer.RegisterSerializer(objectSerializer);

            builder.Services.AddSingleton<IScreenService, ScreenService>();
            builder.Services.AddSingleton<IDevicesService, DevicesService>();
            builder.Services.AddSingleton<IDeviceAuthService, DeviceAuthService>();
            builder.Services.AddSingleton<IAssetService, AssetService>();
            builder.Services.AddSingleton<ITextAssetService, TextAssetService>();
            builder.Services.AddSingleton<IMenuService, MenuService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IUploadService, S3UploadService>();
            builder.Services.AddSingleton<ITemplatesService, TemplatesService>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<ILoginService, LoginService>();
            builder.Services.AddSingleton<IPreviewService, PreviewService>();
            builder.Services.AddSingleton<IPublishService, PublishService>();
            builder.Services.AddSingleton<IAiService, StabilityAiService>();
            builder.Services.AddSingleton<IPlaylistsService, PlaylistsService>();
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddSingleton<IHistoryService, HistoryService>();

            builder.Services.AddSingleton<IContainerClientFactory, ContainerClientFactory>();

            builder.Services.AddSingleton<IDateTimeProvider, SystemDatetimeProvider>();
        }
        
        public static void RegisterNotiicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>(sp =>
            {
                var serviceBusConnectionString = builder.Configuration.GetValue<string>(NotificationConstants.AzureSignalRConnectionStringName);

                return new MessagePublisher(serviceBusConnectionString!, ServiceTransportType.Transient);
            });
        }
    }
}