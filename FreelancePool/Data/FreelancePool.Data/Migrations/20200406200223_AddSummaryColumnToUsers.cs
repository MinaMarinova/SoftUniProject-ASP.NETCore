namespace FreelancePool.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddSummaryColumnToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "AspNetUsers",
                maxLength: 10000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "AspNetUsers");
        }
    }
}
