using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultEmptyRoutesToLoop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Routes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BusId",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoopId",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Entries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_LoopId",
                table: "Routes",
                column: "LoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_StopId",
                table: "Routes",
                column: "StopId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BusId",
                table: "Entries",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_DriverId",
                table: "Entries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_LoopId",
                table: "Entries",
                column: "LoopId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_StopId",
                table: "Entries",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Buses_BusId",
                table: "Entries",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Drivers_DriverId",
                table: "Entries",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Loops_LoopId",
                table: "Entries",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Stops_StopId",
                table: "Entries",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes",
                column: "LoopId",
                principalTable: "Loops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Buses_BusId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Drivers_DriverId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Loops_LoopId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Stops_StopId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Loops_LoopId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Stops_StopId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_LoopId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_StopId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Entries_BusId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_DriverId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_LoopId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_StopId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "BusId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "LoopId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Entries");
        }
    }
}
