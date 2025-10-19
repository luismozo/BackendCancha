using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendCancha.Migrations
{
    /// <inheritdoc />
    public partial class AddReservasTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReservaDetalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReservaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    CanchaId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaDetalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_Canchas_CanchaId",
                        column: x => x.CanchaId,
                        principalTable: "Canchas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReservaDetalle_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Gp4/n5hSsWdm.XX8t0f74.z9hRbgTLW71Z50P/Slzh/v7TvQbbqI.");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioId",
                table: "Reserva",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_CanchaId",
                table: "ReservaDetalle",
                column: "CanchaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_ProductoId",
                table: "ReservaDetalle",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaDetalle_ReservaId",
                table: "ReservaDetalle",
                column: "ReservaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaDetalle");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$QoYb6Or.nI9M5pHXl2gQGuk3AEz4KKRPmMhl4VaTsS2KVHMfgWwW6");
        }
    }
}
