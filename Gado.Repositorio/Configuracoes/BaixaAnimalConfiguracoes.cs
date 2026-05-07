using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gado.Dominio.Entidades;

namespace DataAccess.Configuracoes; // Ajuste para o namespace correto da sua pasta

public class BaixaAnimalConfiguracoes : IEntityTypeConfiguration<BaixaAnimal>
{
    public void Configure(EntityTypeBuilder<BaixaAnimal> builder)
    {
        // Nome da Tabela
        builder.ToTable("BaixasAnimais");

        // Chave Primária
        builder.HasKey(b => b.ID);

        // Propriedades e Validações do Banco
        builder.Property(b => b.Motivo)
               .IsRequired()
               .HasColumnType("varchar(100)");

        builder.Property(b => b.Observacao)
               .HasColumnType("varchar(500)");

        builder.Property(b => b.DataBaixa)
               .IsRequired();

        builder.Property(b => b.Ativo)
               .IsRequired()
               .HasDefaultValue(true);

        // Relacionamento 1 para 1 (Cada baixa pertence a 1 animal)
        builder.HasOne(b => b.Animal)
               .WithMany() // Um animal pode ter várias baixas? Não na vida real, mas no EF deixamos assim para simplificar
               .HasForeignKey(b => b.AnimalID)
               .OnDelete(DeleteBehavior.Restrict); // Não deixa apagar o boi se ele tiver uma baixa registrada
    }
}