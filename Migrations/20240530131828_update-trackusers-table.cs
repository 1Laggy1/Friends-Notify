using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Friends_Notify.Migrations
{
    /// <inheritdoc />
    public partial class updatetrackuserstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackUsers_UserId",
                table: "TrackUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackUsers",
                table: "TrackUsers",
                columns: new[] { "UserId", "TrackingUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackUsers",
                table: "TrackUsers");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUsers_UserId",
                table: "TrackUsers",
                column: "UserId");
        }
    }
}
