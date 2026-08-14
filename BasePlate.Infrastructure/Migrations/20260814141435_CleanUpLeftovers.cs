using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasePlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpLeftovers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utenti_Ristoranti_RistoranteId",
                table: "Utenti");

            migrationBuilder.DropIndex(
                name: "IX_Utenti_RistoranteId",
                table: "Utenti");

            migrationBuilder.DropColumn(
                name: "RistoranteId",
                table: "Utenti");

            migrationBuilder.CreateIndex(
                name: "IX_Utenti_TenantId",
                table: "Utenti",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utenti_Ristoranti_TenantId",
                table: "Utenti",
                column: "TenantId",
                principalTable: "Ristoranti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utenti_Ristoranti_TenantId",
                table: "Utenti");

            migrationBuilder.DropIndex(
                name: "IX_Utenti_TenantId",
                table: "Utenti");

            migrationBuilder.AddColumn<Guid>(
                name: "RistoranteId",
                table: "Utenti",
                type: "uuid",
                nullable: true);

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
    }
}
