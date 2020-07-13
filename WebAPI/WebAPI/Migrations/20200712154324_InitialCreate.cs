using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShaPathDetails",
                columns: table => new
                {
                    FileSha256 = table.Column<string>(nullable: false),
                    FilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShaPathDetails", x => x.FileSha256);
                });

            migrationBuilder.CreateTable(
                name: "FileDetails",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    FileSha256 = table.Column<string>(nullable: true),
                    ShaPathDetailFileSha256 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetails", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileDetails_ShaPathDetails_ShaPathDetailFileSha256",
                        column: x => x.ShaPathDetailFileSha256,
                        principalTable: "ShaPathDetails",
                        principalColumn: "FileSha256",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDetails_ShaPathDetailFileSha256",
                table: "FileDetails",
                column: "ShaPathDetailFileSha256");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDetails");

            migrationBuilder.DropTable(
                name: "ShaPathDetails");
        }
    }
}
