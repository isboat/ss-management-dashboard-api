
using Management.Dashboard.Api.filters;
using Management.Dashboard.Common;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Management.Dashboard.Api
{
    // https://learn.microsoft.com/en-us/azure/container-registry/container-registry-get-started-docker-cli?tabs=azure-cli
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            // Add services to the container.
            RegisterServices(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dashboard Management", Version = "v1" });
                c.AddSecurityDefinition("token", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Scheme = "Bearer"
                });
                // dont add global security requirement
                // c.AddSecurityRequirement(/*...*/);
                c.OperationFilter<SecureEndpointAuthRequirementFilter>();
            });


            AddCustomCorsPolicy(builder, TenantAuthorization.RequiredCorsPolicy);

            RegisterJwtAuth(builder, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseExceptionHandler(ExceptionHandler);

            app.UseHttpsRedirection();

            app.UseCors(TenantAuthorization.RequiredCorsPolicy);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ExceptionHandler(IApplicationBuilder exceptionHandlerApp)
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // using static System.Net.Mime.MediaTypeNames;
                context.Response.ContentType = Text.Plain;

                await context.Response.WriteAsync("An exception was thrown.");

                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                {
                    await context.Response.WriteAsync(" The file was not found.");
                }

                if (exceptionHandlerPathFeature?.Path == "/")
                {
                    await context.Response.WriteAsync(" Page: Home.");
                }
            });
        }

        private static void AddCustomCorsPolicy(WebApplicationBuilder builder, string allowSpecificOrigins)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200",
                                                          "http://localhost:4401",
                                                          "https://wonderful-flower-0b610c010.4.azurestaticapps.net",
                                                          "https://dashboard.onscreensync.com").AllowAnyHeader().AllowAnyMethod();
                                  });
            });
        }

        private static void RegisterServices(WebApplicationBuilder builder)
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
            builder.Services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddSingleton<IPreviewService, PreviewService>();
            builder.Services.AddSingleton<IPublishService, PublishService>();
            builder.Services.AddSingleton<IAiService, StabilityAiService>();
            builder.Services.AddSingleton<IPlaylistsService, PlaylistsService>();
            builder.Services.AddSingleton<IContainerClientFactory, ContainerClientFactory>();

            builder.Services.AddSingleton<IDateTimeProvider, SystemDatetimeProvider>();
        }

        private static void RegisterJwtAuth(WebApplicationBuilder builder, ConfigurationManager configuration)
        {
            var settings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (settings == null) return;

            var isAuthenticationDisabled = false;

            if (!isAuthenticationDisabled)
            {
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(TenantAuthorization.RequiredPolicy, policy =>
                        policy.RequireAuthenticatedUser().RequireClaim("scope", TenantAuthorization.RequiredScope));
                });
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>

                        {
                            options.SaveToken = true;
                            options.TokenValidationParameters = new()
                            {
                                RequireExpirationTime = true,
                                RequireSignedTokens = true,
                                ValidateAudience = true,
                                ValidateIssuer = true,
                                ValidateLifetime = true,

                                // Allow for some drift in server time
                                // (a lower value is better; we recommend two minutes or less)
                                ClockSkew = TimeSpan.FromSeconds(0),

                                ValidIssuer = settings.Issuer,
                                ValidAudience = settings.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SigningKey))
                            };
                        });
            }
            else // authenticate anyone
            {
                //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //    .AddScheme<AuthenticationSchemeOptions, EmptyAuthHandler>
                //        (JwtBearerDefaults.AuthenticationScheme, opts => { });
            }
        }
    }
}