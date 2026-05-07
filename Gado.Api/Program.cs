using DataAccess.Contexto;
using DataAccess.Repositorios;
using Gado.Aplicacao;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; // <--- NECESSÁRIO PARA O FIX DO JSON

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do Banco de Dados (SQLite) ---
// Em produção (Docker), usa a variável de ambiente ConnectionStrings__DefaultConnection
// Em desenvolvimento, usa o caminho local do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=../Gado.Repositorio/gado.sqlite";

builder.Services.AddDbContext<GadoContexto>(options => 
    options.UseSqlite(connectionString));

// --- 2. Injeção de Dependência dos Repositórios ---
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAnimalRepositorio, AnimalRepositorio>();
builder.Services.AddScoped<IVendaRepositorio, VendaRepositorio>();
builder.Services.AddScoped<IMilhoRepositorio, MilhoRepositorio>(); 
builder.Services.AddScoped<IMovimentacaoMilhoRepositorio, MovimentacaoMilhoRepositorio>();
builder.Services.AddScoped<IBaixaAnimalRepositorio, BaixaAnimalRepositorio>(); 

// --- 3. Injeção de Dependência da Aplicação ---
builder.Services.AddScoped<IUsuarioAplicacao, UsuarioAplicacao>();
builder.Services.AddScoped<IAnimalAplicacao, AnimalAplicacao>();
builder.Services.AddScoped<IVendaAplicacao, VendaAplicacao>();
builder.Services.AddScoped<IMilhoAplicacao, MilhoAplicacao>(); 
builder.Services.AddScoped<IMovimentacaoMilhoAplicacao, MovimentacaoMilhoAplicacao>();
builder.Services.AddScoped<IBaixaAnimalAplicacao, BaixaAnimalAplicacao>(); // 

// --- 4. Configuração dos Controllers com Correção de JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // --- O SEGREDO ESTÁ AQUI ---
        // Isso impede que o sistema quebre ao tentar serializar Venda -> Itens -> Venda
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        
        // Aceita JSON em camelCase (email, senha) vindo do front-end React
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        
        // Opcional: Ignorar nulos para deixar o JSON mais limpo
        // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// --- 5. Configuração de CORS (Front-end) ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                  "http://localhost:5173",      // Dev local (React)
                  "http://localhost:3000",      // Dev local (Docker)
                  "http://2.24.83.230:3000"     // Produção (Hostinger)
              )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 6. Configuração do Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 7. Seed: Criar banco e usuário admin padrão (se não existir) ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GadoContexto>();
    
    // Garante que o banco e as tabelas sejam criados
    context.Database.EnsureCreated();
    
    // Se não existir nenhum usuário, cria um admin padrão
    if (!context.Usuarios.Any())
    {
        context.Usuarios.Add(new Gado.Dominio.Entidades.Usuario
        {
            Nome = "Administrador",
            Email = "admin@admin.com",
            Senha = "admin123",
            TipoUsuarioID = Gado.Dominio.Enumeradores.TipoUsuarios.Administrador,
            Ativo = true
        });
        context.SaveChanges();
        Console.WriteLine("✅ Usuário admin padrão criado: admin@admin.com / admin123");
    }
}

// --- 8. Pipeline de Execução ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirecionamento HTTPS (Pode comentar se estiver dando erro de certificado localmente)
app.UseHttpsRedirection();

// IMPORTANTE: UseCors deve vir antes de MapControllers e Authorization
app.UseCors(); 

app.UseAuthorization();

app.MapControllers();

// --- Endpoint de Health Check (usado pelo Docker) ---
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();