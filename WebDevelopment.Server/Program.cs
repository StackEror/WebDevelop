using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebDevelopment.Application;
using WebDevelopment.Application.Security;
using WebDevelopment.Domain.Entities;
using WebDevelopment.Infrastructure;
using WebDevelopment.Shared.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var connectionString = builder.Configuration.GetConnectionString("Default") ??
    throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddUserManager<ApplicationUserManager>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAplication();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<UserContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    options.TokenValidationParameters = JwtHelper.GetTokenValidationParameters(builder.Configuration));

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
