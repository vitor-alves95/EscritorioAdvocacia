using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscritorioAdvocacia.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaTabelaCompromissos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compromissos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdvogadoId = table.Column<int>(type: "int", nullable: false),
                    ProcessoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compromissos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compromissos_Advogados_AdvogadoId",
                        column: x => x.AdvogadoId,
                        principalTable: "Advogados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compromissos_Processos_ProcessoId",
                        column: x => x.ProcessoId,
                        principalTable: "Processos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compromissos_AdvogadoId",
                table: "Compromissos",
                column: "AdvogadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Compromissos_ProcessoId",
                table: "Compromissos",
                column: "ProcessoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compromissos");
        }
    }
}
