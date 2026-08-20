using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BasePlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MenuModifiersSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prodotti_TipologieCottura_TipologiaCotturaId",
                table: "Prodotti");

            migrationBuilder.DropTable(
                name: "TipologieCottura");

            migrationBuilder.DropIndex(
                name: "IX_Prodotti_TipologiaCotturaId",
                table: "Prodotti");

            migrationBuilder.DropColumn(
                name: "TipologiaCotturaId",
                table: "Prodotti");

            migrationBuilder.CreateTable(
                name: "GruppiModificatori",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    InternalDescription = table.Column<string>(type: "text", nullable: false),
                    MinSelections = table.Column<int>(type: "integer", nullable: false),
                    MaxSelections = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruppiModificatori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpzioniModificatore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Sovrapprezzo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    GruppoModificatoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpzioniModificatore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpzioniModificatore_GruppiModificatori_GruppoModificatoreId",
                        column: x => x.GruppoModificatoreId,
                        principalTable: "GruppiModificatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdottoGruppiModificatori",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProdottoId = table.Column<int>(type: "integer", nullable: false),
                    GruppoModificatoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrdineVisualizzazione = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdottoGruppiModificatori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdottoGruppiModificatori_GruppiModificatori_GruppoModific~",
                        column: x => x.GruppoModificatoreId,
                        principalTable: "GruppiModificatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdottoGruppiModificatori_Prodotti_ProdottoId",
                        column: x => x.ProdottoId,
                        principalTable: "Prodotti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpzioniModificatore_GruppoModificatoreId",
                table: "OpzioniModificatore",
                column: "GruppoModificatoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottoGruppiModificatori_GruppoModificatoreId",
                table: "ProdottoGruppiModificatori",
                column: "GruppoModificatoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdottoGruppiModificatori_ProdottoId_GruppoModificatoreId",
                table: "ProdottoGruppiModificatori",
                columns: new[] { "ProdottoId", "GruppoModificatoreId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpzioniModificatore");

            migrationBuilder.DropTable(
                name: "ProdottoGruppiModificatori");

            migrationBuilder.DropTable(
                name: "GruppiModificatori");

            migrationBuilder.AddColumn<int>(
                name: "TipologiaCotturaId",
                table: "Prodotti",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TipologieCottura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descrizione = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipologieCottura", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prodotti_TipologiaCotturaId",
                table: "Prodotti",
                column: "TipologiaCotturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prodotti_TipologieCottura_TipologiaCotturaId",
                table: "Prodotti",
                column: "TipologiaCotturaId",
                principalTable: "TipologieCottura",
                principalColumn: "Id");
        }
    }
}
