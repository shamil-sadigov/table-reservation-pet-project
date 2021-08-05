using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservations.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reservation");

            migrationBuilder.CreateTable(
                name: "Visitor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationRequests",
                schema: "reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClosedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RestaurantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisitorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationRequests_Visitor_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitor",
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
                    ReservationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisitorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Reservations_Visitor_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitor",
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
                name: "IX_Reservations_VisitorId",
                schema: "reservation",
                table: "Reservations",
                column: "VisitorId");
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
                name: "Visitor");
        }
    }
}
