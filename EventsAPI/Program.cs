using EventsAPI.Middlewares;
using Models;
using Repositories;
using EventsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using EventsAPI.ModelProfiles;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using EventsAPI.Validators;
using EventsAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<EventsDbContext>();
builder.Services.AddControllers()
   .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
   });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = false,
            // строка, представляющая издателя
            ValidIssuer = "MyAuthServer",
            // будет ли валидироваться потребитель токена
            ValidateAudience = false,
            // установка потребителя токена,обычно - название сайта
            ValidAudience = "MyAuthClient",
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,

            //RoleClaimType
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IValidator<RegisterModel>,RegisterModelValidator>();
builder.Services.AddScoped<GuestsRepository>();
builder.Services.AddScoped<EventsRepository>();
builder.Services.AddScoped<SharedEventsGuestsRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddAutoMapper(typeof(GuestProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandlerMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
