using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friends_Notify.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackUsers",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    TrackingUserId = table.Column<ulong>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_TrackUsers_Users_TrackingUserId",
                        column: x => x.TrackingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackUsers_TrackingUserId",
                table: "TrackUsers",
                column: "TrackingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUsers_UserId",
                table: "TrackUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
