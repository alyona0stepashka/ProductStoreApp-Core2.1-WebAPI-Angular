using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.DAL.Migrations
{
    public partial class add_logger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log4Nets");

            migrationBuilder.CreateTable(
                name: "LogInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RequestTime = table.Column<DateTime>(nullable: false),
                    RequestURL = table.Column<string>(nullable: true),
                    RequestUserName = table.Column<string>(nullable: true),
                    RequestHeaders = table.Column<string>(nullable: true),
                    RequestBody = table.Column<string>(nullable: true),
                    RequestQueryString = table.Column<string>(nullable: true),
                    RequestHttpVerb = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<DateTime>(nullable: false),
                    RequestURI_r = table.Column<string>(nullable: true),
                    ResponseUserName = table.Column<string>(nullable: true),
                    ResponseHeaders = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<string>(nullable: true),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogInfos");

            migrationBuilder.CreateTable(
                name: "Log4Nets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    RequestBody = table.Column<string>(nullable: true),
                    RequestHeaders = table.Column<string>(nullable: true),
                    RequestHttpVerb = table.Column<string>(nullable: true),
                    RequestQueryString = table.Column<string>(nullable: true),
                    RequestTime = table.Column<DateTime>(nullable: false),
                    RequestURI_r = table.Column<string>(nullable: true),
                    RequestURL = table.Column<string>(nullable: true),
                    RequestUserName = table.Column<string>(nullable: true),
                    ResponseHeaders = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<DateTime>(nullable: false),
                    ResponseUserName = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log4Nets", x => x.Id);
                });
        }
    }
}
