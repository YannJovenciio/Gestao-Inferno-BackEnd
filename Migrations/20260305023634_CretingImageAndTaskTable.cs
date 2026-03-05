using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inferno.Migrations
{
    /// <inheritdoc />
    public partial class CretingImageAndTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HellTask",
                columns: table => new
                {
                    HellTaskId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeadLine = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    DemonId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HellTask", x => x.HellTaskId);
                    table.ForeignKey(
                        name: "FK_HellTask_Demon_DemonId",
                        column: x => x.DemonId,
                        principalTable: "Demon",
                        principalColumn: "IdDemon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HellTask_DemonId",
                table: "HellTask",
                column: "DemonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HellTask");
        }
    }
}
