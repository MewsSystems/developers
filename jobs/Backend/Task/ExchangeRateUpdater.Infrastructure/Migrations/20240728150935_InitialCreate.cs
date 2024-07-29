using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeRateUpdater.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencySources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencySources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CurrencySources",
                columns: new[] { "Id", "CurrencyCode", "SourceUrl" },
                values: new object[] { new Guid("9611d35a-8fbb-4b9b-b90f-092fc196e450"), "CZK", "https://api.cnb.cz/cnbapi/exrates/daily" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencySources");
        }
    }
}
