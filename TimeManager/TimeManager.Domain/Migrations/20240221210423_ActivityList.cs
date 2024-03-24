using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManager.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ActivityList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityListId",
                table: "Activity",
                type: "int",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "ActivityList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityList_User_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityListId",
                table: "Activity",
                column: "ActivityListId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityList_UserId",
                table: "ActivityList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityList_ActivityListId",
                table: "Activity",
                column: "ActivityListId",
                principalTable: "ActivityList",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityList_ActivityListId",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "ActivityList");

            migrationBuilder.DropIndex(
                name: "IX_Activity_ActivityListId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "ActivityListId",
                table: "Activity");
        }
    }
}