using Gado.Dominio.Entidades;
using DataAccess.Repositorios;

namespace Gado.Aplicacao;

public class MilhoAplicacao : IMilhoAplicacao
{
    private readonly IMilhoRepositorio _milhoRepositorio;

    public MilhoAplicacao(IMilhoRepositorio milhoRepositorio)
    {
        _milhoRepositorio = milhoRepositorio;
    }

    public async Task<int> CriarAsync(Milho milho)
    {
        if (milho == null)
            throw new Exception("Dados do milho não podem ser vazios.");

        ValidarInformacoesMilho(milho);

        if (milho.DataCompra == DateTime.MinValue)
            milho.DataCompra = DateTime.Now;

        // Calcula os totais (KG, Dinheiro) e preenche o Estoque Inicial
        milho.CalcularTotais();

        return await _milhoRepositorio.SalvarAsync(milho);
    }

    public async Task AtualizarAsync(Milho milho)
    {
        // Busca a compra original no banco
        var milhoDominio = await _milhoRepositorio.ObterPorIdAsync(milho.ID);

        ValidarInformacoesMilho(milho);

        // --- LÓGICA INTELIGENTE DE ESTOQUE ---
        // Se ele mudou a quantidade de sacos ou o peso, precisamos ajustar o estoque atual 
        // pela diferença do que foi alterado.
        decimal kgCompradoAntigo = milhoDominio.KgComprado;
        
        // Atualiza os campos digitados
        milhoDominio.Vendedor = milho.Vendedor;
        milhoDominio.QuantidadeSacos = milho.QuantidadeSacos;
        milhoDominio.PesoPorSaco = milho.PesoPorSaco; // <--- NOVO CAMPO ADICIONADO AQUI
        milhoDominio.ValorPorSaco = milho.ValorPorSaco;
        milhoDominio.Pagamento = milho.Pagamento;
        milhoDominio.DataCompra = milho.DataCompra;

        // Recalcula o total pago e o total de Kg comprados com os novos dados
        milhoDominio.CalcularTotais();

        // Ajusta o estoque restante baseado na diferença (se comprou mais ou menos na correção)
        decimal diferencaKg = milhoDominio.KgComprado - kgCompradoAntigo;
        milhoDominio.KgEstoqueAtual += diferencaKg; 
        
        // Segurança: o estoque não pode ficar negativo ao ser corrigido
        if (milhoDominio.KgEstoqueAtual < 0)
            throw new Exception("A correção deixaria o estoque negativo. Verifique se não houve consumo deste lote.");

        await _milhoRepositorio.AtualizarAsync(milhoDominio);
    }

    public async Task<Milho> ObterAsync(int id)
    {
        return await _milhoRepositorio.ObterPorIdAsync(id);
    }

    public async Task DeletarAsync(int id)
    {
        var milhoDominio = await _milhoRepositorio.ObterPorIdAsync(id);
        
        // Soft Delete
        milhoDominio.Deletar();

        await _milhoRepositorio.AtualizarAsync(milhoDominio);
    }

    public async Task RestaurarAsync(int id)
    {
        var milhoDominio = await _milhoRepositorio.ObterPorIdAsync(id);

        if (milhoDominio.Ativo)
            throw new Exception("Esta compra de milho já está ativa.");

        milhoDominio.Restaurar();

        await _milhoRepositorio.AtualizarAsync(milhoDominio);
    }

    public async Task<IEnumerable<Milho>> ListarAsync(bool ativo, string termoBusca)
    {
        return await _milhoRepositorio.ListarAsync(ativo, termoBusca);
    }

    #region Util

    private static void ValidarInformacoesMilho(Milho milho)
    {
        if (string.IsNullOrWhiteSpace(milho.Vendedor))
            throw new Exception("O nome do Vendedor é obrigatório.");

        if (milho.QuantidadeSacos <= 0)
            throw new Exception("A quantidade de sacos deve ser maior que zero.");

        if (milho.PesoPorSaco <= 0) 
            throw new Exception("O peso por saco deve ser maior que zero.");

        if (milho.ValorPorSaco <= 0)
            throw new Exception("O valor por saco deve ser maior que zero.");
    }

    #endregion
}