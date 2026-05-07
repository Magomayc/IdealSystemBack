using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gado.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataEntrada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Peso = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    ValorEntrada = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Vendedor = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Sexo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Brinco = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Raca = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PrecoArroba = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Estoque = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Milho",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataCompra = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Vendedor = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    KgComprado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    QuantidadeSacos = table.Column<int>(type: "INTEGER", nullable: false),
                    PesoPorSaco = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ValorPorSaco = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Pagamento = table.Column<int>(type: "INTEGER", nullable: false),
                    KgEstoqueAtual = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milho", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    TipoUsuarioID = table.Column<int>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataVenda = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comprador = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    PrecoArroba = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BaixasAnimais",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataBaixa = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AnimalID = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(500)", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaixasAnimais", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BaixasAnimais_Animais_AnimalID",
                        column: x => x.AnimalID,
                        principalTable: "Animais",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesMilho",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MilhoID = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeKg = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValorVenda = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    ValorPorSacoVendido = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    Pagamento = table.Column<int>(type: "INTEGER", nullable: true),
                    Comprador = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    CustoMovimentacao = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Observacao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesMilho", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MovimentacoesMilho_Milho_MilhoID",
                        column: x => x.MilhoID,
                        principalTable: "Milho",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensVenda",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VendaID = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimalID = table.Column<int>(type: "INTEGER", nullable: false),
                    Raca = table.Column<string>(type: "TEXT", nullable: true),
                    PesoEntrada = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValorEntrada = table.Column<decimal>(type: "TEXT", nullable: false),
                    PesoVivo = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PesoMorto = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    RendimentoCarcaca = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    TotalArrobas = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    ValorUnitario = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensVenda", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Animais_AnimalID",
                        column: x => x.AnimalID,
                        principalTable: "Animais",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Vendas_VendaID",
                        column: x => x.VendaID,
                        principalTable: "Vendas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaixasAnimais_AnimalID",
                table: "BaixasAnimais",
                column: "AnimalID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_AnimalID",
                table: "ItensVenda",
                column: "AnimalID");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_VendaID",
                table: "ItensVenda",
                column: "VendaID");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesMilho_MilhoID",
                table: "MovimentacoesMilho",
                column: "MilhoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaixasAnimais");

            migrationBuilder.DropTable(
                name: "ItensVenda");

            migrationBuilder.DropTable(
                name: "MovimentacoesMilho");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Milho");
        }
    }
}
