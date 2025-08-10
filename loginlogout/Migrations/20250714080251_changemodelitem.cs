using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loginlogout.Migrations
{
    /// <inheritdoc />
    public partial class changemodelitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BannerTitle",
                table: "About",
                newName: "uppercontent");

            migrationBuilder.RenameColumn(
                name: "BannerImage",
                table: "About",
                newName: "Uppertitle");

            migrationBuilder.AddColumn<string>(
                name: "UpperImage",
                table: "About",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpperImage",
                table: "About");

            migrationBuilder.RenameColumn(
                name: "uppercontent",
                table: "About",
                newName: "BannerTitle");

            migrationBuilder.RenameColumn(
                name: "Uppertitle",
                table: "About",
                newName: "BannerImage");
        }
    }
}
