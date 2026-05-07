using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;

namespace DataAccess.Configuracoes;

public class ItemVendaConfiguracoes : IEntityTypeConfiguration<ItemVenda>
{
    public void Configure(EntityTypeBuilder<ItemVenda> builder)
    {
        builder.ToTable("ItensVenda");

        builder.HasKey(i => i.ID);

        // --- RELACIONAMENTOS ---
        builder.HasOne(i => i.Animal)
            .WithMany()
            .HasForeignKey(i => i.AnimalID)
            .OnDelete(DeleteBehavior.Restrict); // Não deixa deletar um boi se ele já foi vendido

        // --- PESOS E MEDIDAS DO FRIGORÍFICO ---
        builder.Property(i => i.PesoVivo)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(i => i.PesoMorto)
            .HasPrecision(18, 2); // Não tem IsRequired() porque pode estar vazio até o frigorífico mandar o romaneio

        builder.Property(i => i.RendimentoCarcaca)
            .HasPrecision(18, 2); // Pode ser nulo inicialmente

        builder.Property(i => i.TotalArrobas)
            .HasPrecision(18, 2); // Pode ser nulo inicialmente

        builder.Property(i => i.ValorUnitario)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}