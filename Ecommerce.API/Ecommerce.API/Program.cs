using BuildingBlocks.EndPoints;
using ECommerce.Application;
using ECommerce.Application.Models.Mail;
using ECommerce.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // OAuth2 Configuration for Swagger UI
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:7110/connect/authorize"),
                TokenUrl = new Uri("https://localhost:7110/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID" },
                    { "profile", "Profile" },
                    { "email", "Email" },
                    { "ecommerceapi", "Ecommerce API" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "openid", "profile", "email", "ecommerceapi" } // Required scopes
        }
    });
});

builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastuctureServices(builder.Configuration);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(EmailSettings.SectionName));
//builder.Services.AddSingleton<IRedisDbProvider>(provider =>
//{
//    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
//    return new RedisDbProvider(redisConnectionString);
//});
//builder.Services.AddSingleton<IDistributedCache, RedisCacheHandler>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    {
        EndPoints = { "127.0.0.1:6379" },
        ConnectTimeout = 10000, // Increase from 5000ms to 10000ms
        SyncTimeout = 10000,
        AbortOnConnectFail = false // Prevents crashes if Redis is down
    };
});


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer" , options =>
    {
        options.Authority = "https://localhost:7110";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseGlobalExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.ProductEndpoints();
app.OrderEndpoints();
app.CategoryEndpoints();
app.MapControllers();
app.CustomerEndpoints();
app.Run();
