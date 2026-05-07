namespace Gado.Dominio.Enumeradores;

public enum TipoMovimentacaoMilho
{
    Consumo = 1, // Foi para o cocho (Gera custo de produção)
    Venda = 2,   // Vendeu para terceiros (Gera receita)
    Perda = 3    // Estragou, molhou, etc. (Gera custo/prejuízo)
}