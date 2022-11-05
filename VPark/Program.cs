using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using VPark.Extensions;
using VPark_Core.Repositories.Implementation;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;

var builder = WebApplication.CreateBuilder(args);

#region Serilog Configuration

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
    path: "logs\\log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();
try
{
    Log.Information("VPark Application is Starting..");
    //builder.Build().Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "VPark Application Failed to start");
}
finally
{
    Log.CloseAndFlush();
}
builder.Host.UseSerilog();

#endregion

// Add services to the container.
builder.Services.ResolveSwagger();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
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
