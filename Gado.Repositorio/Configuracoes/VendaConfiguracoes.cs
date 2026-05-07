using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;
using Gado.Dominio.Enumeradores; // Adicionamos a referência para a pasta de enumeradores

namespace DataAccess.Configuracoes;

public class VendaConfiguracoes : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");

        builder.HasKey(v => v.ID);

        builder.Property(v => v.DataVenda)
            .IsRequired();

        // AJUSTE: Transformamos em opcional, pois em uma 'Baixa' não há comprador
        builder.Property(v => v.Comprador)
            .HasMaxLength(150)
            .IsRequired(false); 

        // --- NOVO: Configuração do Enumerador ---
        builder.Property(v => v.Tipo)
            .HasConversion<int>() // Garante que será salvo como 1 ou 2 no SQL
            .IsRequired()
            .HasDefaultValue(TipoBaixa.Venda); // Mantém o histórico compatível

        // --- VALORES FINANCEIROS ---
        builder.Property(v => v.PrecoArroba)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(v => v.ValorTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(v => v.Ativo)
            .HasDefaultValue(true);

        // --- RELACIONAMENTO ---
        // Uma Venda tem muitos ItensVenda. Se deletar a Venda, deleta os itens (Cascade)
        builder.HasMany(v => v.Itens)
            .WithOne(i => i.Venda)
            .HasForeignKey(i => i.VendaID)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}