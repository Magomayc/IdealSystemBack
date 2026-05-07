using Gado.Dominio.Entidades;
using DataAccess.Repositorios;
using Gado.Dominio.Enumeradores;

namespace Gado.Aplicacao;

public class MovimentacaoMilhoAplicacao : IMovimentacaoMilhoAplicacao
{
    private readonly IMovimentacaoMilhoRepositorio _movimentacaoRepositorio;
    private readonly IMilhoRepositorio _milhoRepositorio;

    public MovimentacaoMilhoAplicacao(
        IMovimentacaoMilhoRepositorio movimentacaoRepositorio,
        IMilhoRepositorio milhoRepositorio)
    {
        _movimentacaoRepositorio = movimentacaoRepositorio;
        _milhoRepositorio = milhoRepositorio;
    }

    public async Task<int> CriarAsync(MovimentacaoMilho movimentacao)
    {
        ValidarInformacoes(movimentacao);

        // 1. Busca o lote de milho original para verificar se tem estoque
        var milhoOrigem = await _milhoRepositorio.ObterPorIdAsync(movimentacao.MilhoID);

        if (milhoOrigem.KgEstoqueAtual < movimentacao.QuantidadeKg)
            throw new Exception($"Estoque insuficiente neste lote! Você tentou retirar {movimentacao.QuantidadeKg}kg, mas só restam {milhoOrigem.KgEstoqueAtual}kg.");

        // 2. Calcula o custo da movimentação com base no preço que foi pago
        movimentacao.CalcularFinanceiro(milhoOrigem);

        // 3. Abate do estoque principal
        milhoOrigem.KgEstoqueAtual -= movimentacao.QuantidadeKg;

        if (movimentacao.DataMovimentacao == DateTime.MinValue)
            movimentacao.DataMovimentacao = DateTime.Now;

        // 4. Salva a movimentação e atualiza o milho
        var id = await _movimentacaoRepositorio.SalvarAsync(movimentacao);
        await _milhoRepositorio.AtualizarAsync(milhoOrigem);

        return id;
    }

    public async Task AtualizarAsync(MovimentacaoMilho movimentacao)
    {
        var movimentacaoBanco = await _movimentacaoRepositorio.ObterPorIdAsync(movimentacao.ID);
        var milhoOrigem = await _milhoRepositorio.ObterPorIdAsync(movimentacaoBanco.MilhoID);

        ValidarInformacoes(movimentacao);

        // Calcula a diferença de KG caso o usuário tenha digitado errado e esteja corrigindo
        decimal diferencaKg = movimentacao.QuantidadeKg - movimentacaoBanco.QuantidadeKg;

        // Verifica se a correção não vai deixar o estoque principal negativo
        if (milhoOrigem.KgEstoqueAtual - diferencaKg < 0)
            throw new Exception("Esta alteração deixaria o estoque do lote negativo.");

        // Atualiza o estoque do milho original
        milhoOrigem.KgEstoqueAtual -= diferencaKg;

        // Atualiza os dados da movimentação
        movimentacaoBanco.Tipo = movimentacao.Tipo;
        movimentacaoBanco.QuantidadeKg = movimentacao.QuantidadeKg;
        movimentacaoBanco.DataMovimentacao = movimentacao.DataMovimentacao;
        
        // --- CAMPOS DE VENDA ---
        movimentacaoBanco.ValorVenda = movimentacao.ValorVenda;
        movimentacaoBanco.ValorPorSacoVendido = movimentacao.ValorPorSacoVendido; 
        movimentacaoBanco.Pagamento = movimentacao.Pagamento; 
        movimentacaoBanco.Comprador = movimentacao.Comprador;
        
        movimentacaoBanco.Observacao = movimentacao.Observacao;

        // Recalcula o custo financeiro com a nova quantidade
        movimentacaoBanco.CalcularFinanceiro(milhoOrigem);

        // Salva as duas tabelas
        await _movimentacaoRepositorio.AtualizarAsync(movimentacaoBanco);
        await _milhoRepositorio.AtualizarAsync(milhoOrigem);
    }

    public async Task<MovimentacaoMilho> ObterAsync(int id)
    {
        return await _movimentacaoRepositorio.ObterPorIdAsync(id);
    }

    public async Task DeletarAsync(int id)
    {
        var movimentacaoBanco = await _movimentacaoRepositorio.ObterPorIdAsync(id);
        
        if (movimentacaoBanco == null)
            throw new Exception("Movimentação não encontrada.");

        var milhoOrigem = await _milhoRepositorio.ObterPorIdAsync(movimentacaoBanco.MilhoID);

        if (milhoOrigem != null)
        {
            // O milho "volta" para o armazém original
            milhoOrigem.KgEstoqueAtual += movimentacaoBanco.QuantidadeKg;
            
            // ATUALIZAÇÃO SEGURA: Garantimos a atualização do estoque do milho PRIMEIRO
            await _milhoRepositorio.AtualizarAsync(milhoOrigem);
        }

        // Deleta (Soft Delete) a movimentação
        movimentacaoBanco.Deletar();

        // Atualiza a movimentação no banco como inativa
        await _movimentacaoRepositorio.AtualizarAsync(movimentacaoBanco);
    }

    public async Task RestaurarAsync(int id)
    {
        var movimentacaoBanco = await _movimentacaoRepositorio.ObterPorIdAsync(id);
        
        if (movimentacaoBanco == null)
            throw new Exception("Movimentação não encontrada.");

        if (movimentacaoBanco.Ativo)
            throw new Exception("Esta movimentação já está ativa.");

        var milhoOrigem = await _milhoRepositorio.ObterPorIdAsync(movimentacaoBanco.MilhoID);

        if (milhoOrigem != null)
        {
            // Se restaurou o uso, tira do armazém de novo
            if (milhoOrigem.KgEstoqueAtual < movimentacaoBanco.QuantidadeKg)
                throw new Exception("Não é possível restaurar, pois não há estoque suficiente no lote de origem.");

            milhoOrigem.KgEstoqueAtual -= movimentacaoBanco.QuantidadeKg;
            
            // Garantimos a atualização do estoque do milho PRIMEIRO
            await _milhoRepositorio.AtualizarAsync(milhoOrigem);
        }
        
        movimentacaoBanco.Restaurar();

        // Atualiza a movimentação no banco como ativa
        await _movimentacaoRepositorio.AtualizarAsync(movimentacaoBanco);
    }

    public async Task<IEnumerable<MovimentacaoMilho>> ListarAsync(bool ativo, int? milhoId = null)
    {
        return await _movimentacaoRepositorio.ListarAsync(ativo, milhoId);
    }

    #region Util

    private static void ValidarInformacoes(MovimentacaoMilho movimentacao)
    {
        if (movimentacao.MilhoID <= 0)
            throw new Exception("É obrigatório informar de qual compra de milho esta saída pertence.");

        if (movimentacao.QuantidadeKg <= 0)
            throw new Exception("A quantidade movimentada deve ser maior que zero.");

        // --- VALIDAÇÕES EXCLUSIVAS DE VENDA (BARRANDO ERROS) ---
        if (movimentacao.Tipo == TipoMovimentacaoMilho.Venda)
        {
            if (movimentacao.ValorVenda == null || movimentacao.ValorVenda <= 0)
                throw new Exception("Para movimentações do tipo Venda, o Valor Total da Venda é obrigatório.");

            if (movimentacao.ValorPorSacoVendido == null || movimentacao.ValorPorSacoVendido <= 0)
                throw new Exception("Para movimentações do tipo Venda, o Valor por Saco é obrigatório.");

            if (movimentacao.Pagamento == null)
                throw new Exception("Para movimentações do tipo Venda, a Forma de Pagamento é obrigatória.");
                
            if (string.IsNullOrWhiteSpace(movimentacao.Comprador))
                throw new Exception("Para movimentações do tipo Venda, informar o Comprador é obrigatório.");
        }
    }

    #endregion
}