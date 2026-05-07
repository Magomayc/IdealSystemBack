using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;

namespace DataAccess.Configuracoes;

public class MovimentacaoMilhoConfiguracoes : IEntityTypeConfiguration<MovimentacaoMilho>
{
    public void Configure(EntityTypeBuilder<MovimentacaoMilho> builder)
    {
        builder.ToTable("MovimentacoesMilho");

        builder.HasKey(m => m.ID);

        // --- RELACIONAMENTO (CHAVE ESTRANGEIRA) ---
        builder.HasOne(m => m.Milho)
               .WithMany() // Um milho pode ter várias movimentações
               .HasForeignKey(m => m.MilhoID)
               .OnDelete(DeleteBehavior.Restrict); // Evita deletar um Milho que já tem movimentações

        builder.Property(m => m.DataMovimentacao)
            .IsRequired();

        builder.Property(m => m.Tipo)
            .IsRequired()
            .HasConversion<int>(); // Salva o Enum como número (1, 2 ou 3)

        // --- CAMPOS DECIMAIS (Peso e Dinheiro) ---
        builder.Property(m => m.QuantidadeKg)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.CustoMovimentacao)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(m => m.ValorVenda)
            .HasPrecision(18, 2); // Pode ser nulo, então não tem IsRequired()

        // --- NOVOS CAMPOS DE VENDA ---
        builder.Property(m => m.ValorPorSacoVendido)
            .HasPrecision(18, 2); // Pode ser nulo
            
        builder.Property(m => m.Pagamento)
            .HasConversion<int?>(); // Salva o Enum como número no banco, mas permite nulo

        // --- CAMPOS DE TEXTO ---
        builder.Property(m => m.Comprador)
            .HasMaxLength(150);

        builder.Property(m => m.Observacao)
            .HasMaxLength(500);

        builder.Property(m => m.Ativo)
            .HasDefaultValue(true);
    }
}