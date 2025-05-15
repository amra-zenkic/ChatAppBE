namespace ChatAppBE
{
    using ChatAppBE.Hubs;
    using ChatAppBE.Models.Models;
    using ChatAppBE.Services.Services;
    using ChatAppBE.Services.Services.IService;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        private const string AllowAllOrigins = "AllowAllOrigins";

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var mongoDbSection = builder.Configuration.GetSection("MongoDBSettings");
            var mongoDBSettings = mongoDbSection.Get<MongoDBSettings>();
            builder.Services.Configure<MongoDBSettings>(mongoDbSection);

            builder.Services.AddDbContext<ChatAppDbContext>(options =>
                options.UseMongoDB(mongoDBSettings?.AtlasURI ?? string.Empty, mongoDBSettings?.DatabaseName ?? string.Empty));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMessagesService, MessagesService>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowAllOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/Chat");

            app.UseCors(AllowAllOrigins);

            app.Run();
        }
    }
}