using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscritorioAdvocacia.Migrations
{
    /// <inheritdoc />
    public partial class LinkAdvogadoToUse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Advogados",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advogados_ApplicationUserId",
                table: "Advogados",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advogados_AspNetUsers_ApplicationUserId",
                table: "Advogados",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advogados_AspNetUsers_ApplicationUserId",
                table: "Advogados");

            migrationBuilder.DropIndex(
                name: "IX_Advogados_ApplicationUserId",
                table: "Advogados");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Advogados");
        }
    }
}
