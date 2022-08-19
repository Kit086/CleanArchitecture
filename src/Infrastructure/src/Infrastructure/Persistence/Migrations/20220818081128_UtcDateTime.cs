using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Infrastructure.src.Infrastructure.Persistence.Migrations
{
    public partial class UtcDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "TodoLists",
                newName: "LastModifiedUtc");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "TodoLists",
                newName: "CreatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "TodoItems",
                newName: "LastModifiedUtc");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "TodoItems",
                newName: "CreatedUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedUtc",
                table: "TodoLists",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "TodoLists",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "LastModifiedUtc",
                table: "TodoItems",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "TodoItems",
                newName: "Created");
        }
    }
}
