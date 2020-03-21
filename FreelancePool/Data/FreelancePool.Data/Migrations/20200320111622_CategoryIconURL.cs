namespace FreelancePool.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CategoryIconURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "IconURL",
                table: "Categories",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconURL",
                table: "Categories");

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "Categories",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
