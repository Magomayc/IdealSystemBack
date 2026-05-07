namespace Gado.Dominio.Entidades;

public class ItemVenda
{
    public int ID { get; set; }

    // Link com a Venda (Pai)
    public int VendaID { get; set; }
    public virtual Venda Venda { get; set; }

    // Link com o Animal (Filho)
    public int AnimalID { get; set; }
    public virtual Animal Animal { get; set; }

    // --- SNAPSHOT (FOTOGRAFIA DO PASSADO) ---
    // Guarda os dados originais do animal no momento da venda. 
    // Assim, se o animal for alterado depois, o relatório de venda não muda!
    public string Raca { get; set; }
    public decimal PesoEntrada { get; set; }
    public decimal ValorEntrada { get; set; }

    // --- DADOS DA BALANÇA E FRIGORÍFICO ---
    public decimal PesoVivo { get; set; } // Peso na fazenda antes de embarcar
    public decimal? PesoMorto { get; set; } // Nulo até o frigorífico devolver o romaneio de abate
    
    // --- CÁLCULOS AUTOMÁTICOS DO SISTEMA ---
    public decimal? RendimentoCarcaca { get; private set; } // Ex: 52%
    public decimal? TotalArrobas { get; private set; } // PesoMorto / 15
    public decimal ValorUnitario { get; private set; } // Arrobas * PrecoArroba

    // --- MÉTODOS INTELIGENTES ---
    public void ProcessarRomaneio(decimal precoArroba)
    {
        // Só faz a conta de abate se o usuário já tiver preenchido o Peso Morto
        if (PesoMorto.HasValue && PesoVivo > 0)
        {
            // 1. Calcula a % de aproveitamento da carne
            RendimentoCarcaca = Math.Round((PesoMorto.Value / PesoVivo) * 100, 2);
            
            // 2. Descobre quantas arrobas deu exatas (Peso morto / 15kg)
            TotalArrobas = Math.Round(PesoMorto.Value / 15m, 2);
            
            // 3. Calcula o valor financeiro final deste boi
            ValorUnitario = Math.Round(TotalArrobas.Value * precoArroba, 2);
        }
        else
        {
            // Se ainda não tem o peso morto, o valor fica zerado aguardando o frigorífico
            ValorUnitario = 0;
            RendimentoCarcaca = null;
            TotalArrobas = null;
        }
    }
}