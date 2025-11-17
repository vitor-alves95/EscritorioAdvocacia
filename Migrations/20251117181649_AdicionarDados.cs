using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EscritorioAdvocacia.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VarasOrigem",
                columns: new[] { "Id", "Comarca", "Nome" },
                values: new object[,]
                {
                    { 1, "São Paulo", "1ª Vara Cível" },
                    { 2, "Campinas", "Vara de Família e Sucessões" },
                    { 3, "Rio de Janeiro", "Vara do Trabalho" },
                    { 4, "Belo Horizonte", "Juizado Especial Cível" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VarasOrigem",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VarasOrigem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "VarasOrigem",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "VarasOrigem",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
