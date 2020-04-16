namespace FreelancePool.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddProjectOffersUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPost_Categories_CategoryId",
                table: "CategoryPost");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPost_Posts_PostId",
                table: "CategoryPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ApplicationUserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ApplicationUserId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryPost",
                table: "CategoryPost");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "CategoryPost",
                newName: "CategoriesPosts");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryPost_PostId",
                table: "CategoriesPosts",
                newName: "IX_CategoriesPosts_PostId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriesPosts",
                table: "CategoriesPosts",
                columns: new[] { "CategoryId", "PostId" });

            migrationBuilder.CreateTable(
                name: "ProjectOffersUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectOffersUsers", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectOffersUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectOffersUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectOffersUsers_ProjectId",
                table: "ProjectOffersUsers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesPosts_Categories_CategoryId",
                table: "CategoriesPosts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesPosts_Posts_PostId",
                table: "CategoriesPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesPosts_Categories_CategoryId",
                table: "CategoriesPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesPosts_Posts_PostId",
                table: "CategoriesPosts");

            migrationBuilder.DropTable(
                name: "ProjectOffersUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriesPosts",
                table: "CategoriesPosts");

            migrationBuilder.RenameTable(
                name: "CategoriesPosts",
                newName: "CategoryPost");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesPosts_PostId",
                table: "CategoryPost",
                newName: "IX_CategoryPost_PostId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5000);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryPost",
                table: "CategoryPost",
                columns: new[] { "CategoryId", "PostId" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ApplicationUserId",
                table: "Projects",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPost_Categories_CategoryId",
                table: "CategoryPost",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPost_Posts_PostId",
                table: "CategoryPost",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ApplicationUserId",
                table: "Projects",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
