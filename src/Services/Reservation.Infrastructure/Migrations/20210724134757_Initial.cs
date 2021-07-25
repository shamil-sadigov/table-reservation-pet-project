using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.Infrastructure.Databass.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reservation");

            migrationBuilder.CreateTable(
                name: "Restaurants",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartWorkingAt = table.Column<TimeSpan>(type: "time", precision: 0, scale: 0, nullable: false),
                    FinishWorkingAt = table.Column<TimeSpan>(type: "time", precision: 0, scale: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfSeats = table.Column<byte>(type: "tinyint", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tables_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalSchema: "reservation",
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRequests",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitingTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_Tables_TableId",
                        column: x => x.TableId,
                        principalSchema: "reservation",
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_TableId",
                schema: "reservation",
                table: "ReservationRequests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_RestaurantId",
                schema: "reservation",
                table: "Tables",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationRequests",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Tables",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Restaurants",
                schema: "reservation");
        }
    }
}
