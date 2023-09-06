using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudeWeather.Report.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempLowF",
                table: "Weather_report",
                newName: "RinfallTotalInches");

            migrationBuilder.RenameColumn(
                name: "TempHighF",
                table: "Weather_report",
                newName: "AverageLowF");

            migrationBuilder.RenameColumn(
                name: "RainfallTotalInches",
                table: "Weather_report",
                newName: "AverageHighF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RinfallTotalInches",
                table: "Weather_report",
                newName: "TempLowF");

            migrationBuilder.RenameColumn(
                name: "AverageLowF",
                table: "Weather_report",
                newName: "TempHighF");

            migrationBuilder.RenameColumn(
                name: "AverageHighF",
                table: "Weather_report",
                newName: "RainfallTotalInches");
        }
    }
}
