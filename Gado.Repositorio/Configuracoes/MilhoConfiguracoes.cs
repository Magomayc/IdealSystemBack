using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;

namespace DataAccess.Configuracoes;

public class MilhoConfiguracoes : IEntityTypeConfiguration<Milho>
{
    public void Configure(EntityTypeBuilder<Milho> builder)
    {
        builder.ToTable("Milho"); // Nome da tabela no banco

        builder.HasKey(m => m.ID);

        builder.Property(m => m.DataCompra)
            .IsRequired();

        builder.Property(m => m.Vendedor)
            .HasMaxLength(150)
            .IsRequired();

        // --- VALORES DECIMAIS (Peso e Dinheiro) ---
        // HasPrecision(18, 2) garante que o banco aceite números grandes com 2 casas decimais

        builder.Property(m => m.PesoPorSaco) // <--- Coloquei aqui junto com os volumes
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.KgComprado)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.KgEstoqueAtual)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.ValorPorSaco)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.ValorTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        // --- INTEIROS E ENUMS ---

        builder.Property(m => m.QuantidadeSacos)
            .IsRequired();

        builder.Property(m => m.Pagamento)
            .IsRequired()
            .HasConversion<int>(); // Salva o Enum como número no banco

        builder.Property(m => m.Ativo)
            .HasDefaultValue(true);
    }
}