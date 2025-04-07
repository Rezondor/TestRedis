namespace TestRedis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var redisConn = builder.Configuration["REDIS_CONNECTION"];

        if (!string.IsNullOrEmpty(redisConn))
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConn;
            });
            Console.WriteLine($"Using Redis cache: {redisConn}");
        }
        else
        {
            builder.Services.AddDistributedMemoryCache();
            Console.WriteLine("Using in-memory cache");
        }

        var app = builder.Build();

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
