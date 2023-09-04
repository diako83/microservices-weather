using Microsoft.EntityFrameworkCore;

namespace CloudeWeather.Report.DataAcess;

public class WeatherReportDbContext:DbContext
{
    protected WeatherReportDbContext()
    {
    }

    public WeatherReportDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WeatherReport> WeatherReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentityTableNames(modelBuilder);
    }

    private  static void SnakeCaseIdentityTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherReport>(b =>
        {
            b.ToTable("Weather_report");
        });
    }
}