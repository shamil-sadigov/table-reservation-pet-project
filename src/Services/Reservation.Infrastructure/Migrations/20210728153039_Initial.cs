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
                name: "Visitors",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumberOfSeats = table.Column<byte>(type: "tinyint", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfRequestedSeats = table.Column<byte>(type: "tinyint", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_ReservationRequests_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalSchema: "reservation",
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRequestRejections",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RejectedByAdministratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RejectionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRequestRejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationRequestRejections_ReservationRequests_ReservationRequestId",
                        column: x => x.ReservationRequestId,
                        principalSchema: "reservation",
                        principalTable: "ReservationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedByAdministratorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_ReservationRequests_ReservationRequestId",
                        column: x => x.ReservationRequestId,
                        principalSchema: "reservation",
                        principalTable: "ReservationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequestRejections_ReservationRequestId",
                schema: "reservation",
                table: "ReservationRequestRejections",
                column: "ReservationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_TableId",
                schema: "reservation",
                table: "ReservationRequests",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRequests_VisitorId",
                schema: "reservation",
                table: "ReservationRequests",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationRequestId",
                schema: "reservation",
                table: "Reservations",
                column: "ReservationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tables_RestaurantId",
                schema: "reservation",
                table: "Tables",
                column: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationRequestRejections",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Reservations",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "ReservationRequests",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Tables",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Visitors",
                schema: "reservation");

            migrationBuilder.DropTable(
                name: "Restaurants",
                schema: "reservation");
        }
    }
}
