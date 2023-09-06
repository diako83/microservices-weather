using CloudeWeather.Report.DataAcess;

namespace CloudeWeather.Report.BusinesLogic;

public interface IWeatherReportAggregator
{
    public Task<WeatherReport> BuildReport(string zip, int days);
}