
using CacheTutorial.Common;
using Microsoft.Extensions.Configuration;

namespace CacheTutorial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            var redisOptions = new RedisOptions();
            builder.Configuration.GetSection("Redis").Bind(redisOptions);
            // Conditionally add Redis caching if it's enabled
            if (redisOptions.IsEnable)
            {
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisOptions.DefaultConn;
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                    {
                        EndPoints = { redisOptions.DefaultConn },
                        AbortOnConnectFail = false,
                        ConnectTimeout = redisOptions.DefaultTimeOut * 1000 // Convert seconds to milliseconds
                    };
                });
            }

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
    }
}
