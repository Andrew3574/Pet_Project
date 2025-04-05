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
            // ���������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = false,
            // ������, �������������� ��������
            ValidIssuer = "MyAuthServer",
            // ����� �� �������������� ����������� ������
            ValidateAudience = false,
            // ��������� ����������� ������,������ - �������� �����
            ValidAudience = "MyAuthClient",
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,
            // ��������� ����� ������������
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
            // ��������� ����� ������������
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
