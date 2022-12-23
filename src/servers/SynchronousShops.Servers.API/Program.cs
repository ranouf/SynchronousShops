using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SynchronousShops.Domains.Infrastructure.SQLServer;
using SynchronousShops.Libraries.Settings;
using SynchronousShops.Servers.API;
using SynchronousShops.Servers.API.Filters;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server
builder.Services.AddDbContext<SKSQLDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        opt => opt.EnableRetryOnFailure()
    )
);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<APIModule>();
});

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddControllers(options =>
    {
        //options.EnableEndpointRouting = false;
        options.Filters.Add(typeof(ValidateModelStateAttribute));
    })
    .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
         options.SerializerSettings.Converters.Add(new StringEnumConverter());
         options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Home";
        await next();
    }
});

// Last operations before running the API
ValidateSettings(app.Services);
await InitializeDataBasesAsync(app.Services);

app.Run();

#region Private

static void ValidateSettings(IServiceProvider services)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Starting settings validation.");
        services.GetRequiredService<SettingsValidator>();
        logger.LogInformation("The settings has been validated.");
    }
    catch (Exception e)
    {
        logger.LogError("An error occurred while validating the settings.", e);
    }
}

static async Task InitializeDataBasesAsync(IServiceProvider services)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Starting the SQLServerDB initialization.");
        await DbInitializer.InitializeAsync(services, logger);
        logger.LogInformation("The SQLServerDB initialization has been done.");
    }
    catch (Exception e)
    {
        logger.LogError("An error occurred while initialization the SQLServerDB.", e);
        throw;
    }
}

#endregion

public partial class Program { }