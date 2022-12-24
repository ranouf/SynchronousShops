
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling;
using SynchronousShops.Domains.Core.Identity.Configuration;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Domains.Infrastructure.SqlServer;
using SynchronousShops.Libraries.Authentication.Extensions;
using SynchronousShops.Libraries.Constants;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Libraries.Settings;
using SynchronousShops.Libraries.Settings.HealthChecks;
using SynchronousShops.Libraries.SMTP.Configuration;
using SynchronousShops.Libraries.SMTP.HealthChecks;
using SynchronousShops.Servers.API;
using SynchronousShops.Servers.API.Configuration;
using SynchronousShops.Servers.API.Filters;
using SynchronousShops.Servers.API.Middlewares;
using SynchronousShops.Servers.API.SignalR;
using System;
using System.IO;
using System.Threading.Tasks;

var corsPolicy = "CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

////Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        corsPolicy,
        builder => builder
            .WithOrigins(
                "http://localhost:4200",
                "https://allinone-app-dev.azurewebsites.net/"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
    );
});

// Dependancy Injection
builder.Services.AddAutofac();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<APIModule>();
});

// SQL Server
builder.Services.AddDbContext<SynchronousShopsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        opt => opt.EnableRetryOnFailure()
    )
);

// Identity
builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
        options.Lockout.AllowedForNewUsers = false;
    })
    .AddEntityFrameworkStores<SynchronousShopsDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, SynchronousShopsDbContext, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>()
    .AddRoleStore<RoleStore<Role, SynchronousShopsDbContext, Guid, UserRole, IdentityRoleClaim<Guid>>>();

//Caching response for middlewares
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

// Authentication
builder.Services.RegisterAuthentication();

// SignalR
builder.Services.AddSignalR()
    .AddNewtonsoftJsonProtocol();
builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

// Insights
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);

// Add framework builder.Services.
builder.Services.AddControllers(options =>
{
    //options.EnableEndpointRouting = false;
    options.Filters.AddService(typeof(ApiExceptionFilter));
    options.Filters.Add(typeof(ValidateModelStateAttribute));
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

// Swagger-ui 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"{Project.Name} API",
        Version = "v1",
        Description = $"Welcome to the marvellous {Project.Name} API!",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new[] { "readAccess", "writeAccess" }
        }
    });
});
builder.Services.AddSwaggerDocument();

// HealthCheck
builder.Services.AddHealthChecksUI(setupSettings: options =>
{
    var settings = HealthCheckSettings.FromConfiguration(builder.Configuration);
    if (settings != null)
    {
        options.AddHealthCheckEndpoint(
            settings.Name,
            settings.Uri
        );
        options.SetEvaluationTimeInSeconds(settings.EvaluationTimeinSeconds);
        options.SetMinimumSecondsBetweenFailureNotifications(settings.MinimumSecondsBetweenFailureNotifications);
    }
}).AddInMemoryStorage();

builder.Services.AddHealthChecks()
    .AddCheck<SmtpHealthCheck>("SMTP")
    .AddAzureBlobStorage(builder.Configuration["AzureStorage:ConnectionString"])
    .AddCheckSettings<IdentitySettings>()
    .AddCheckSettings<SmtpSettings>()
    .AddDbContextCheck<SynchronousShopsDbContext>("Default");

// Profiling
builder.Services.AddMemoryCache();
builder.Services.AddMiniProfiler(options =>
{
    options.PopupRenderPosition = RenderPosition.Left;
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Auto;
});

// Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Controllers
builder.Services.AddControllers();

// Settings
builder.Services
    .AddOptions()
    .ConfigureAndValidate<IdentitySettings>(builder.Configuration)
    .ConfigureAndValidate<SmtpSettings>(builder.Configuration);

var app = builder.Build();

//Cors
app.UseCors(corsPolicy);

app.UseRouting();

// Https
app.UseHttpsRedirection();
app.UseHsts();

app.UseDefaultFiles();
app.UseStaticFiles();

//Authentication
app.UseAuthentication();
app.UseAuthorization();

//Caching
app.UseResponseCaching();

// Swagger-ui
app.UseSwagger(c => c.RouteTemplate = "api-endpoints/{documentName}/swagger.json");
app.UseSwaggerUi3(c =>
{
    c.Path = "/api-endpoints";
    c.DocumentPath = "/api-endpoints/{documentName}/swagger.json";
});
//app.UseSwaggerUI(c =>
//{
//    c.RoutePrefix = "api-endpoints";
//    c.SwaggerEndpoint("v1/swagger.json", $"{Constants.Project.Name} V1");
//    c.DisplayRequestDuration();
//    c.DefaultModelsExpandDepth(-1); 
//    c.DocExpansion(DocExpansion.None);
//    c.IndexStream = () => Assembly.GetEntryAssembly().GetManifestResourceStream($"{Constants.Project.Name}.Servers.API.Assets.SwaggerIndex.html");

//});

// Profiling, url to see last profile check: http://localhost:XXXX/profiler/results
app.UseMiniProfiler();
app.UseMiddleware<RequestMiddleware>();

// HealthCheck
app.UseHealthChecks("/healthchecks", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(config =>
{
    config.UIPath = "/api-healthchecks";
});

app.UseEndpoints(endpoints =>
{
    // SignalR
    endpoints.MapHub<GlobalHub>(Api.V1.Hub.Url);

    // Controllers
    endpoints.MapControllers();
});

// Redirect any non-API calls to the Angular application
// so our application can handle the routing
app.Use(async (context, next) =>
{
    await next();
    // Avoid 404 error for AlwaysOn signal in azure
    if (context.Request.Path == "/online/ping")
    {
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync("Server ON");
    }
    else
    {
        if (context.Response.StatusCode == 404
            && !Path.HasExtension(context.Request.Path.Value)
            && !context.Request.Path.Value.StartsWith("/api/"))
        {
            context.Request.Path = "/index.html";
            await next();
        }
    }
});

// Last operations before running the API
await InitializeDataBasesAsync(app.Services);
GenerateAzureSettings(app.Services);

await app.RunAsync();


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

static void GenerateAzureSettings(IServiceProvider services)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Start to generate Azure settings.");
        var azureSettings = AzureSettingsGenerator.Generate(services);
        logger.LogInformation(azureSettings);
        logger.LogInformation("Azure settings have been generated.");
    }
    catch (Exception ex)
    {
        logger.LogError("An error occurred while validating Azure settings.", ex);
        throw;
    }
}

public partial class Program { }
