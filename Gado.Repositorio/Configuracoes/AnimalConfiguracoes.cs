using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;
// Caso queira forçar a conversão explícita com o tipo, não esqueça do using:
// using Gado.Dominio.Enumeradores; 

namespace DataAccess.Configuracoes;

public class AnimalConfiguracoes : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable("Animais");

        builder.HasKey(a => a.ID);

        builder.Property(a => a.ID).HasColumnName("ID");

        builder.Property(a => a.DataEntrada)
            .HasColumnName("DataEntrada")
            .IsRequired();

        builder.Property(a => a.Peso)
            .HasColumnName("Peso")
            .HasPrecision(10, 2) 
            .IsRequired();

        builder.Property(a => a.ValorEntrada)
            .HasColumnName("ValorEntrada")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(a => a.Vendedor)
            .HasColumnName("Vendedor")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(a => a.Ativo)
            .HasColumnName("Ativo")
            .HasDefaultValue(true); 
        
        builder.Property(a => a.Sexo)
            .HasColumnName("Sexo")
            .HasMaxLength(20); 

        builder.Property(a => a.Brinco)
            .HasColumnName("Brinco")
            .HasMaxLength(50); 

        builder.Property(a => a.Raca)
            .HasColumnName("Raca")
            .HasMaxLength(100); 

        builder.Property(a => a.PrecoArroba)
            .HasColumnName("PrecoArroba")
            .HasPrecision(18, 2); 
            
        builder.Property(a => a.Estoque)
            .HasColumnName("Estoque")
            .IsRequired()
            .HasConversion<int>(); 
    }
}