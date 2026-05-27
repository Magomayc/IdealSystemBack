using Microsoft.EntityFrameworkCore;
using Gado.Dominio.Entidades;
using DataAccess.Configuracoes;

namespace DataAccess.Contexto;

public class GadoContexto : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Milho> Milhos { get; set; } 
    
    // --- DBSET DA MOVIMENTAÇÃO ---
    public DbSet<MovimentacaoMilho> MovimentacoesMilho { get; set; }
    
    // --- VENDAS ---
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; } 

    // --- NOVO: BAIXA DE ANIMAIS (Mortes/Perdas) ---
    public DbSet<BaixaAnimal> BaixasAnimais { get; set; }

    public GadoContexto()
    {
    }

    public GadoContexto(DbContextOptions<GadoContexto> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=GadoDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
        modelBuilder.ApplyConfiguration(new AnimalConfiguracoes());
        modelBuilder.ApplyConfiguration(new VendaConfiguracoes());
        modelBuilder.ApplyConfiguration(new ItemVendaConfiguracoes()); 
        modelBuilder.ApplyConfiguration(new MilhoConfiguracoes()); 
        modelBuilder.ApplyConfiguration(new MovimentacaoMilhoConfiguracoes());
        
        // --- NOVA CONFIGURAÇÃO DA BAIXA ---
        modelBuilder.ApplyConfiguration(new BaixaAnimalConfiguracoes());
    }
}