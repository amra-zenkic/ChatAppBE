using ChatAppBE.Hubs;
using ChatAppBE.Models.Models;
using ChatAppBE.Services;
using ChatAppBE.Services.Services;
using ChatAppBE.Services.Services.IService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Add MongoDB settings
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddDbContext<ChatAppDbContext>(options =>
    options.UseMongoDB(mongoDBSettings.AtlasURI ?? "", mongoDBSettings.DatabaseName ?? ""));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});




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
app.MapHub<ChatHub>("/Chat"); // endpoint for SignalR hub

app.UseCors("AllowAllOrigins");

app.Run();
