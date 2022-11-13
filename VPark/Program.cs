using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using VPark.Extensions;
using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Models.Models;

var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

#endregion

// Add services to the container.
builder.Services.ResolveSwagger();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = true;
    opt.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>();
//builder.Services.AddAuthentication(auth =>
//{
//    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(opt =>
//{
//    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,        
//        ValidAudience = builder.Configuration["AuthSettings:Audience"],
//        ValidIssuer = builder.Configuration["C:Issuer"],
//        RequireExpirationTime = true,   
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Key"])),
//        ValidateIssuerSigningKey = true
//    };
//});
builder.Services.ResolveDependencyInjectionServices();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
