using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Point.Of.Sale.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OutBoxEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutBoxedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DomainEvent = table.Column<int>(type: "integer", nullable: false),
                    TotalTask = table.Column<int>(type: "integer", nullable: false),
                    CompletedTask = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ScheduledOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinalizedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    BatchErrors = table.Column<List<string>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutBoxedEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutBoxedEvents_Id_PublishedOn_Status",
                table: "OutBoxedEvents",
                columns: new[] { "Id", "PublishedOn", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutBoxedEvents");
        }
    }
}
