using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBlob.Migrations
{
    public partial class ModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDetail_Id",
                table: "BlobSummary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileDetail_Id",
                table: "BlobSummary",
                nullable: false,
                defaultValue: 0);
        }
    }
}
