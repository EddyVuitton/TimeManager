﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManager.Domain.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabaseObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HourType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepetitionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepetitionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repetition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepetitionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repetition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repetition_RepetitionType_RepetitionTypeId",
                        column: x => x.RepetitionTypeId,
                        principalTable: "RepetitionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTime>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourTypeId = table.Column<int>(type: "int", nullable: false),
                    RepetitionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_HourType_HourTypeId",
                        column: x => x.HourTypeId,
                        principalTable: "HourType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activity_Repetition_RepetitionId",
                        column: x => x.RepetitionId,
                        principalTable: "Repetition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_HourTypeId",
                table: "Activity",
                column: "HourTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_RepetitionId",
                table: "Activity",
                column: "RepetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserId",
                table: "Activity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Repetition_RepetitionTypeId",
                table: "Repetition",
                column: "RepetitionTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "HourType");

            migrationBuilder.DropTable(
                name: "Repetition");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "RepetitionType");
        }
    }
}