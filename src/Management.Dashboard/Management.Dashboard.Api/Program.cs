
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Management.Dashboard.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            // Add services to the container.
            RegisterServices(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            AddCustomCorsPolicy(builder, TenantAuthorization.RequiredCorsPolicy);

            RegisterJwtAuth(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(TenantAuthorization.RequiredCorsPolicy);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void AddCustomCorsPolicy(WebApplicationBuilder builder, string allowSpecificOrigins)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200",
                                                          "http://www.contoso.com").AllowAnyHeader().AllowAnyMethod();
                                  });
            });
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRepository<ScreenModel>, ScreenRepository>();
            builder.Services.AddSingleton<IRepository<MenuModel>, MenuRepository>();
            builder.Services.AddSingleton<ITemplatesRepository, TemplatesRepository>();
            builder.Services.AddSingleton<IUserRepository<UserModel>, UserRepository>();
            builder.Services.AddSingleton<IRepository<AssetItemModel>, AssetRepository>();

            builder.Services.AddSingleton<IScreenService, ScreenService>();
            builder.Services.AddSingleton<IAssetService, AssetService>();
            builder.Services.AddSingleton<IMenuService, MenuService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IUploadService, LocalUploadService>();
            builder.Services.AddSingleton<ITemplatesService, TemplatesService>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            builder.Services.AddSingleton<IContainerClientFactory, ContainerClientFactory>();
        }

        private static void RegisterJwtAuth(WebApplicationBuilder builder)
        {
            var jwtIssuer = "http://mysite.com";
            var jwtAudience = "http://myaudience.com";
            var jwtSigningKey = "asdv234234^&%&^%&^hjsdfb2%%%";

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

                                ValidIssuer = jwtIssuer,
                                ValidAudience = jwtAudience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSigningKey))
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