using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatserver.Migrations
{
    /// <inheritdoc />
    public partial class addAttributeAvatarForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAva",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAva",
                table: "Users");
        }
    }
}
