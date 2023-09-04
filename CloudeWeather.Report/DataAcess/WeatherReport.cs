namespace CloudeWeather.Report.DataAcess;

public class WeatherReport
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal TempHighF { get; set; }
    public decimal TempLowF { get; set; }
    public decimal RainfallTotalInches { get; set; }
    public decimal SnowTotalInches { get; set; }
    public string ZipCode { get; set; }
}