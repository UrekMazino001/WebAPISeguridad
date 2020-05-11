using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPISeguridad.Migrations
{
    public partial class Empresa_Sigla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12");

            migrationBuilder.AddColumn<string>(
                name: "Siglas",
                table: "Empresas",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1212121", "c5cb1501-c2ca-4bda-b774-fcd04aef48d7", "contador", "contador" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1212121");

            migrationBuilder.DropColumn(
                name: "Siglas",
                table: "Empresas");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "12", "bdbdc026-c03a-45e2-89e6-936862ca069f", "admin", "admin" });
        }
    }
}
