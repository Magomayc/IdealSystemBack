using Gado.Dominio.Entidades;
using DataAccess.Repositorios;
using Gado.Dominio.Enumeradores; // <--- Importando o seu Enum

namespace Gado.Aplicacao;

public class AnimalAplicacao : IAnimalAplicacao
{
    private readonly IAnimalRepositorio _animalRepositorio;

    public AnimalAplicacao(IAnimalRepositorio animalRepositorio)
    {
        _animalRepositorio = animalRepositorio;
    }

    public async Task<int> CriarAsync(Animal animal)
    {
        if (animal == null)
            throw new Exception("Dados do animal não podem ser vazios.");

        ValidarInformacoesAnimal(animal);

        if (animal.DataEntrada == DateTime.MinValue)
            animal.DataEntrada = DateTime.Now;

        return await _animalRepositorio.SalvarAsync(animal);
    }

    public async Task AtualizarAsync(Animal animal)
    {
        // Busca o animal existente
        var animalDominio = await _animalRepositorio.ObterPorIdAsync(animal.ID);

        ValidarInformacoesAnimal(animal);

        // Atualiza os campos antigos
        animalDominio.Peso = animal.Peso;
        animalDominio.ValorEntrada = animal.ValorEntrada;
        animalDominio.Vendedor = animal.Vendedor;
        animalDominio.DataEntrada = animal.DataEntrada;

        // --- ATUALIZA OS NOVOS CAMPOS E O ESTOQUE ---
        animalDominio.Sexo = animal.Sexo;
        animalDominio.Brinco = animal.Brinco;
        animalDominio.Raca = animal.Raca;
        animalDominio.PrecoArroba = animal.PrecoArroba;
        animalDominio.Estoque = animal.Estoque; // <--- Atualizando o local do gado

        await _animalRepositorio.AtualizarAsync(animalDominio);
    }

    public async Task<Animal> ObterAsync(int id)
    {
        return await _animalRepositorio.ObterPorIdAsync(id);
    }

    public async Task DeletarAsync(int id)
    {
        var animalDominio = await _animalRepositorio.ObterPorIdAsync(id);
        
        // Soft Delete (apenas desativa)
        animalDominio.Deletar();

        await _animalRepositorio.AtualizarAsync(animalDominio);
    }

    public async Task RestaurarAsync(int id)
    {
        var animalDominio = await _animalRepositorio.ObterPorIdAsync(id);

        if (animalDominio.Ativo)
            throw new Exception("Este animal já está ativo.");

        animalDominio.Restaurar();

        await _animalRepositorio.AtualizarAsync(animalDominio);
    }

    // --- REPASSANDO O FILTRO DE ESTOQUE PARA O REPOSITÓRIO ---
    public async Task<IEnumerable<Animal>> ListarAsync(bool ativo, string termoBusca, TipoEstoque? tipoEstoque = null)
    {
        return await _animalRepositorio.ListarAsync(ativo, termoBusca, tipoEstoque);
    }

    #region Util

    private static void ValidarInformacoesAnimal(Animal animal)
    {
        if (animal.Peso <= 0)
        {
            throw new Exception("O Peso do animal deve ser maior que zero.");
        }

        if (animal.ValorEntrada < 0)
        {
            throw new Exception("O Valor de Entrada não pode ser negativo.");
        }

        if (string.IsNullOrEmpty(animal.Vendedor))
        {
            throw new Exception("O nome do Vendedor é obrigatório.");
        }

        if (animal.PrecoArroba < 0)
        {
            throw new Exception("O Preço da Arroba não pode ser negativo.");
        }
    }

    #endregion
}