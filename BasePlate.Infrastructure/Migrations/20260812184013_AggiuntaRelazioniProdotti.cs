using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasePlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaRelazioniProdotti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdottoAllergene",
                columns: table => new
                {
                    ProdottoId = table.Column<int>(type: "integer", nullable: false),
                    AllergeneId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdottoAllergene", x => new { x.ProdottoId, x.AllergeneId });
                    table.ForeignKey(
                        name: "FK_ProdottoAllergene_Allergeni_AllergeneId",
                        column: x => x.AllergeneId,
                        principalTable: "Allergeni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdottoAllergene_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdottoIngrediente",
                columns: table => new
                {
                    ProdottoId = table.Column<int>(type: "integer", nullable: false),
                    IngredienteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdottoIngrediente", x => new { x.ProdottoId, x.IngredienteId });
                    table.ForeignKey(
                        name: "FK_ProdottoIngrediente_Ingredienti_IngredienteId",
                        column: x => x.IngredienteId,
                        principalTable: "Ingredienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdottoIngrediente_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdottoAllergene_AllergeneId",
                table: "ProdottoAllergene",
                column: "AllergeneId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottoIngrediente_IngredienteId",
                table: "ProdottoIngrediente",
                column: "IngredienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdottoAllergene");

            migrationBuilder.DropTable(
                name: "ProdottoIngrediente");
        }
    }
}
