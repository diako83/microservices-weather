using System.Text.Json;
using CloudeWeather.Report.Config;
using CloudeWeather.Report.DataAcess;
using CloudeWeather.Report.Models;
using Microsoft.Extensions.Options;

namespace CloudeWeather.Report.BusinesLogic;

public class WeatherReportAggregator: IWeatherReportAggregator
{
    private readonly IHttpClientFactory _http;
    private readonly ILogger<WeatherReportAggregator> _logger;
    private readonly WeatherDataConfig _weatherDataConfig;
    private readonly WeatherReportDbContext _db;

    public WeatherReportAggregator(
        IHttpClientFactory http,
        ILogger<WeatherReportAggregator> logger,
        WeatherDataConfig weatherDataConfig, 
        WeatherReportDbContext db)
    {
        _http = http;
        _logger = logger;
        _weatherDataConfig = weatherDataConfig;
        _db = db;
    }

    public async Task<WeatherReport> BuildReport(string zip, int days)
    {
        var httpClient = _http.CreateClient();
        
        var precipData = await FetchPrecipitationData(httpClient, zip, days);
        var totalSnow = GetTotalSnow(precipData);
        var totaRain = GetTotalRain(precipData);
        
        _logger.LogInformation(
            $"zip:{zip} over last {days} days: " +
            $"total snow: {totalSnow}, rain:{totaRain}"
            );
        
        var tempData = await FetchTemperatureData(httpClient, zip, days);
        var averageHighTemp = tempData.Average(t => t.TempHighF);
        var averageLowTemp = tempData.Average(t => t.TempLowF);

        _logger.LogInformation(
            $"zip:{zip} over last {days} days: " +
            $"lo temp: {averageLowTemp}, hi temp:{averageHighTemp}"
        );
        var weatherReport = new WeatherReport
        {
            AverageHighF = Math.Round(averageHighTemp, 1),
            AverageLowF = Math.Round(averageLowTemp, 1),
            RinfallTotalInches = totalSnow,
            SnowTotalInches = totalSnow,
            ZipCode = zip,
            CreatedOn = DateTime.UtcNow,

        };
        
        //Toodo: Use 'cahse weatehr report instead of making round trips when posible
        _db.Add(weatherReport);
        await _db.SaveChangesAsync();
        return weatherReport;
    }

    private static decimal GetTotalSnow(IEnumerable<PrecipitationModel> precipData)
    {
        var totalSnow = precipData
            .Where(p => p.WeatherType == "snow")
            .Sum(p => p.AmountInches);
        return Math.Round(totalSnow, 1);
    }  
    private static decimal GetTotalRain(IEnumerable<PrecipitationModel> precipData)
    {
        var totalSnow = precipData
            .Where(p => p.WeatherType == "rain")
            .Sum(p => p.AmountInches);
        return Math.Round(totalSnow, 1);
    }

    private async Task<List<PrecipitationModel>> FetchPrecipitationData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildPrecipitationEndpoint(zip, days);
        var precipRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };


        var priceData = await precipRecords
            .Content
            .ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializerOptions);
        return priceData ?? new List<PrecipitationModel>();
    }
    
    private async Task<List<TemperatureModel>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildTemperatureServiceEndpoint(zip, days);
        var temperatureRecords = await httpClient.GetAsync(endpoint);
        
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var temperatureData = await temperatureRecords
            .Content
            .ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializerOptions);
        return temperatureData ?? new List<TemperatureModel>();
    }

    private string? BuildTemperatureServiceEndpoint(string zip, int days)
    {
        var tempServiceProtocol = _weatherDataConfig.TempDataPort;
        var tempServiceHost = _weatherDataConfig.TempDataHost;
        var tempServiceHostPort = _weatherDataConfig.TempDataPort;
        return $"{tempServiceProtocol}://{tempServiceHost}:{tempServiceHostPort}/observational/{zip}?days={days}";
    }

    private string BuildPrecipitationEndpoint(string zip, int days)
    {
        var tempServiceProtocol = _weatherDataConfig.TempDataPort;
        var tempServiceHost = _weatherDataConfig.TempDataHost;
        var tempServiceHostPort = _weatherDataConfig.TempDataPort;
        return $"{tempServiceProtocol}://{tempServiceHost}:{tempServiceHostPort}/observational/{zip}?days={days}";
    }


}


