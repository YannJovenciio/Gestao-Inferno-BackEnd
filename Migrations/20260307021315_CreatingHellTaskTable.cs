using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inferno.Migrations
{
    /// <inheritdoc />
    public partial class CreatingHellTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HellTask_Demon_DemonId",
                table: "HellTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HellTask",
                table: "HellTask");

            migrationBuilder.RenameTable(
                name: "HellTask",
                newName: "HellTasks");

            migrationBuilder.RenameIndex(
                name: "IX_HellTask_DemonId",
                table: "HellTasks",
                newName: "IX_HellTasks_DemonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HellTasks",
                table: "HellTasks",
                column: "HellTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_HellTasks_Demon_DemonId",
                table: "HellTasks",
                column: "DemonId",
                principalTable: "Demon",
                principalColumn: "IdDemon",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HellTasks_Demon_DemonId",
                table: "HellTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HellTasks",
                table: "HellTasks");

            migrationBuilder.RenameTable(
                name: "HellTasks",
                newName: "HellTask");

            migrationBuilder.RenameIndex(
                name: "IX_HellTasks_DemonId",
                table: "HellTask",
                newName: "IX_HellTask_DemonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HellTask",
                table: "HellTask",
                column: "HellTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_HellTask_Demon_DemonId",
                table: "HellTask",
                column: "DemonId",
                principalTable: "Demon",
                principalColumn: "IdDemon",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
