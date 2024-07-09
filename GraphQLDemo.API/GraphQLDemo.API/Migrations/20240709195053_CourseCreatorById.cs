using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GraphQLDemo.API.Migrations
{
    public partial class CourseCreatorById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseDTOId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CourseDTOId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseDTOId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Courses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseDTOStudentDTO",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDTOStudentDTO", x => new { x.CoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseDTOStudentDTO_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDTOStudentDTO_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDTOStudentDTO_StudentsId",
                table: "CourseDTOStudentDTO",
                column: "StudentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDTOStudentDTO");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Courses");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseDTOId",
                table: "Students",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseDTOId",
                table: "Students",
                column: "CourseDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseDTOId",
                table: "Students",
                column: "CourseDTOId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
