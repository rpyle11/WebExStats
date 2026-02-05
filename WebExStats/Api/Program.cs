
using Stats.Api.Entities;
using Stats.Api.Models;
using Stats.Api.Services;
using System.Net;

namespace Stats.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
       
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSqlServer<WebExStatsContext>(builder.Configuration.GetConnectionString("DbCnn"));
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<IRepositoryService, RepositoryService>();
        builder.Services.AddScoped<IDataService, DataService>();
        builder.Services.AddHttpClient<IWebExApiCalls, WebExApiCalls>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("AppSettings:WebExApiBaseUrl").Value!);
        });
        builder.Services.AddHttpClient<ILogService, LogService>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("AppSettings:LogUrl").Value!);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            Credentials = CredentialCache.DefaultNetworkCredentials
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}