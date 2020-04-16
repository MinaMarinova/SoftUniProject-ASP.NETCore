namespace FreelancePool.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CreateMessagesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_AspNetUsers_AuthorId",
                table: "Recommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendation_AspNetUsers_RecipientId",
                table: "Recommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendation",
                table: "Recommendation");

            migrationBuilder.RenameTable(
                name: "Recommendation",
                newName: "Recommendations");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendation_RecipientId",
                table: "Recommendations",
                newName: "IX_Recommendations_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendation_IsDeleted",
                table: "Recommendations",
                newName: "IX_Recommendations_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendation_AuthorId",
                table: "Recommendations",
                newName: "IX_Recommendations_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    AuthorId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IsDeleted",
                table: "Messages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ProjectId",
                table: "Messages",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_AspNetUsers_AuthorId",
                table: "Recommendations",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_AspNetUsers_RecipientId",
                table: "Recommendations",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_AspNetUsers_AuthorId",
                table: "Recommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_AspNetUsers_RecipientId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Recommendations",
                table: "Recommendations");

            migrationBuilder.RenameTable(
                name: "Recommendations",
                newName: "Recommendation");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_RecipientId",
                table: "Recommendation",
                newName: "IX_Recommendation_RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_IsDeleted",
                table: "Recommendation",
                newName: "IX_Recommendation_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Recommendations_AuthorId",
                table: "Recommendation",
                newName: "IX_Recommendation_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Recommendation",
                table: "Recommendation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_AspNetUsers_AuthorId",
                table: "Recommendation",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendation_AspNetUsers_RecipientId",
                table: "Recommendation",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
