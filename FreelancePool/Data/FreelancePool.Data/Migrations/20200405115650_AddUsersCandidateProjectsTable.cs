namespace FreelancePool.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUsersCandidateProjectsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersCandidateProjects",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCandidateProjects", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UsersCandidateProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersCandidateProjects_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersCandidateProjects_ProjectId",
                table: "UsersCandidateProjects",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersCandidateProjects");
        }
    }
}
