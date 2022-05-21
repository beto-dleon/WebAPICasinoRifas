using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPICasinoRifas.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rifas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disponibilidad = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rifas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Premios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disponibilidad = table.Column<bool>(type: "bit", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    RifaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Premios_Rifas_RifaId",
                        column: x => x.RifaId,
                        principalTable: "Rifas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RifasConParticipantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParticipanteId = table.Column<int>(type: "int", nullable: false),
                    RifaId = table.Column<int>(type: "int", nullable: false),
                    NumeroLoteria = table.Column<int>(type: "int", nullable: false),
                    PremioId = table.Column<int>(type: "int", nullable: false),
                    Ganador = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RifasConParticipantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RifasConParticipantes_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalTable: "Participantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RifasConParticipantes_Rifas_RifaId",
                        column: x => x.RifaId,
                        principalTable: "Rifas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Premios_RifaId",
                table: "Premios",
                column: "RifaId");

            migrationBuilder.CreateIndex(
                name: "IX_RifasConParticipantes_ParticipanteId",
                table: "RifasConParticipantes",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_RifasConParticipantes_RifaId",
                table: "RifasConParticipantes",
                column: "RifaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Premios");

            migrationBuilder.DropTable(
                name: "RifasConParticipantes");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "Rifas");
        }
    }
}
