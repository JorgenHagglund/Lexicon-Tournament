using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
using Tournament.Api.Extensions;
using Tournament.Core.Contracts;
using Tournament.Data.Data;
using Tournament.Data.Repositories;
using Tournament.Services;
using Tournament.Services.Extensions;

namespace Tournament.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TournamentContext>(options =>
                options
                .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                .UseSqlServer(builder.Configuration.GetConnectionString("TournamentContext") ??
                    throw new InvalidOperationException("Connection string 'TournamentContext' not found.")));

            // Create services to the container.

            builder.Services
                .AddControllers(opt =>
                {
                    opt.ReturnHttpNotAcceptable = false;
                    opt.Filters.Add<MetaDataFilterAttribute>();
                    opt.Filters.Add<ErrorFilterAttribute>();
                })
                .AddNewtonsoftJson()
                .AddApplicationPart(typeof(AssemblyReference).Assembly)
                .AddXmlDataContractSerializerFormatters();

            builder.Services.ConfigureServiceLayerServices();
            builder.Services.ConfigureRepositories();

            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
