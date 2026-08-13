using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasePlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCampiProdotto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredienteProdotto");

            migrationBuilder.AddColumn<string>(
                name: "Descrizione",
                table: "Prodotti",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImmagineUrl",
                table: "Prodotti",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredienteId",
                table: "Prodotti",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_IngredienteId",
                table: "Prodotti",
                column: "IngredienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_Ingredienti_IngredienteId",
                table: "Prodotti",
                column: "IngredienteId",
                principalTable: "Ingredienti",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_Ingredienti_IngredienteId",
                table: "Prodotti");

            migrationBuilder.DropIndex(
                name: "IX_Prodotti_IngredienteId",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "Descrizione",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "ImmagineUrl",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "IngredienteId",
                table: "Prodotti");

            migrationBuilder.CreateTable(
                name: "IngredienteProdotto",
                columns: table => new
                {
                    IngredientiId = table.Column<int>(type: "integer", nullable: false),
                    ProdottiId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredienteProdotto", x => new { x.IngredientiId, x.ProdottiId });
                    table.ForeignKey(
                        name: "FK_IngredienteProdotto_Ingredienti_IngredientiId",
                        column: x => x.IngredientiId,
                        principalTable: "Ingredienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredienteProdotto_Prodotti_ProdottiId",
                        column: x => x.ProdottiId,
                        principalTable: "Prodotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredienteProdotto_ProdottiId",
                table: "IngredienteProdotto",
                column: "ProdottiId");
        }
    }
}
