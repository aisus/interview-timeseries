using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSeriesStorage.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetricsEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MachineId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            // Convert MetricsEntries Table to Hypertable
            migrationBuilder.Sql(
                "SELECT create_hypertable( '\"MetricsEntries\"', 'Timestamp');\n" +
                "CREATE INDEX ix_status_timestamp ON \"MetricsEntries\" (\"Status\", \"Timestamp\" DESC);\n" + 
                "CREATE INDEX ix_machineid_timestamp ON \"MetricsEntries\" (\"MachineId\", \"Timestamp\" DESC);\n"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricsEntries");
        }
    }
}
