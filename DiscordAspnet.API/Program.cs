using DiscordAspnet.Application.Interfaces;
using DiscordAspnet.Application.Services;
using DiscordAspnet.Domain.Adapters;
using DiscordAspnet.Domain.Entities;
using DiscordAspnet.Domain.Repositories;
using DiscordAspnet.Infrastructure.Adapters;
using DiscordAspnet.Infrastructure.Context;
using DiscordAspnet.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.ContainsKey("Sec-WebSocket-Protocol") && context.HttpContext.WebSockets.IsWebSocketRequest)
                        {
                            var token = context.Request.Headers["Sec-WebSocket-Protocol"].ToString();
                            Console.WriteLine(token);
                            // token arrives as string = "client, xxxxxxxxxxxxxxxxxxxxx"
                            context.Token = token.Substring(token.IndexOf(',') + 1).Trim();
                            context.Request.Headers["Sec-WebSocket-Protocol"] = "client";
                        }
                        return Task.CompletedTask;
                    }
                };

            });
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IGuildRepository, GuildRepository>();
builder.Services.AddScoped<IGuildService, GuildService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddSingleton<IWebSocketAdapter, WebSocketAdapter>();
builder.Services.AddSingleton<IWebSocketService, WebSocketService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

app.UseWebSockets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
