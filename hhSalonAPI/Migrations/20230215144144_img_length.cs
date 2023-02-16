using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhSalonAPI.Migrations
{
    public partial class img_length : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "groups_of_services",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "groups_of_services",
                keyColumn: "img_url",
                keyValue: null,
                column: "img_url",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "img_url",
                table: "groups_of_services",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
