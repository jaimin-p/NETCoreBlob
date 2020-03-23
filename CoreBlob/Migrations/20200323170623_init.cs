using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBlob.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MyProperty = table.Column<string>(maxLength: 100, nullable: false),
                    Summary = table.Column<string>(maxLength: 500, nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlobSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    FileDetail_Id = table.Column<int>(nullable: false),
                    Blob = table.Column<byte[]>(nullable: true),
                    FileDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlobSummary_FileDetail_FileDetailId",
                        column: x => x.FileDetailId,
                        principalTable: "FileDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlobSummary_FileDetailId",
                table: "BlobSummary",
                column: "FileDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlobSummary");

            migrationBuilder.DropTable(
                name: "FileDetail");
        }
    }
}
