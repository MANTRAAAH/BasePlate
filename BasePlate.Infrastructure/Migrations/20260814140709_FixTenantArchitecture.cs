using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasePlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTenantArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RistoranteId",
                table: "Allergeni");

            migrationBuilder.AddColumn<Guid>(
                name: "RistoranteId",
                table: "Utenti",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Utenti",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Ristoranti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    PartitaIva = table.Column<string>(type: "text", nullable: false),
                    DominioPersonalizzato = table.Column<string>(type: "text", nullable: false),
                    TemaLayout = table.Column<string>(type: "text", nullable: false),
                    ColorePrimario = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ristoranti", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_RistoranteId",
                table: "Utenti",
                column: "RistoranteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utenti_Ristoranti_RistoranteId",
                table: "Utenti",
                column: "RistoranteId",
                principalTable: "Ristoranti",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utenti_Ristoranti_RistoranteId",
                table: "Utenti");

            migrationBuilder.DropTable(
                name: "Ristoranti");

            migrationBuilder.DropIndex(
                name: "IX_Utenti_RistoranteId",
                table: "Utenti");

            migrationBuilder.DropColumn(
                name: "RistoranteId",
                table: "Utenti");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Utenti");

            migrationBuilder.AddColumn<int>(
                name: "RistoranteId",
                table: "Allergeni",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
