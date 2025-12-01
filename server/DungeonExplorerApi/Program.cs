using DungeonExplorerApi.Endpoints;
using DungeonExplorerApi.Handlers;
using DungeonExplorerApi.Helpers;
using DungeonExplorerApi.Middleware;
using DungeonExplorerApi.Repository;

namespace DungeonExplorerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            services.AddSqlite<DataContext>("DataSource=DungeonExplorerApi.db");

            // Add services to the container.
            services.AddScoped<IMapHandler, MapHandler>();
            services.AddScoped<IMapRepository, MapRepository>();

            services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(x =>
            {
                x.CustomSchemaIds(type => type.FullName);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalErrorMiddleware>();

            MapsEndpoint.Map(app.MapGroup("/api"));

            app.Run();
        }
    }
}
