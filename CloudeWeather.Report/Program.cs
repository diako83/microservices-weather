using System.IO.Compression;
using CloudeWeather.Report.BusinesLogic;
using CloudeWeather.Report.Config;
using CloudeWeather.Report.DataAcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.AddOptions();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));


builder.Services.AddDbContext<WeatherReportDbContext>(
    opts =>
    {
        opts.EnableSensitiveDataLogging();
        opts.EnableDetailedErrors();
        opts.UseNpgsql(builder.Configuration.GetConnectionString("AppDb"));
    },
    ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet(
    "/weather-report/{zip}",
    async (string zip, [FromQuery] int? days, IWeatherReportAggregator weatherAgg) =>
    {
        
        if (days == null || days < 1 || days > 30)
        {
            return Results.BadRequest("Please provice a 'days' qvalue between 1 and 30 days ");
        }
       var report = await weatherAgg.BuildReport(zip, days.Value);
       return Results.Ok(report);
    }
);


app.Run();