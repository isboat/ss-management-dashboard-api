
using Management.Dashboard.Models;
using Management.Dashboard.Repositories;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services;
using Management.Dashboard.Services.Interfaces;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRepository<ScreenModel>, ScreenRepository>();
            builder.Services.AddSingleton<IRepository<MenuModel>, MenuRepository>();
            builder.Services.AddSingleton<ITemplatesRepository, TemplatesRepository>();

            builder.Services.AddSingleton<IScreenService, ScreenService>();
            builder.Services.AddSingleton<IMenuService, MenuService>();
            builder.Services.AddSingleton<ITemplatesService, TemplatesService>();
        }
    }
}