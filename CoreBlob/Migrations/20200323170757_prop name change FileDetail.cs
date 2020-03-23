using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreBlob.Migrations
{
    public partial class propnamechangeFileDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "FileDetail",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FileDetail",
                newName: "MyProperty");
        }
    }
}
