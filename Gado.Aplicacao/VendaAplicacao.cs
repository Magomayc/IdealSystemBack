using Gado.Dominio.Entidades;
using DataAccess.Repositorios;
using Gado.Aplicacao.DTOs;
using Gado.Dominio.Enumeradores; // AJUSTE 1: Importamos o seu novo enumerador!

namespace Gado.Aplicacao;

public class VendaAplicacao : IVendaAplicacao
{
    private readonly IVendaRepositorio _vendaRepositorio;
    private readonly IAnimalRepositorio _animalRepositorio;

    public VendaAplicacao(IVendaRepositorio vendaRepositorio, IAnimalRepositorio animalRepositorio)
    {
        _vendaRepositorio = vendaRepositorio;
        _animalRepositorio = animalRepositorio;
    }

    public async Task<Venda> CriarAsync(RegistrarVendaDTO dto)
    {
        // 1. Validações Iniciais
        if (dto.Itens == null || !dto.Itens.Any())
            throw new Exception("Selecione pelo menos um animal para a operação.");

        // AJUSTE 2: A MÁGICA ACONTECE AQUI!
        // Só exige comprador e preço maior que zero SE for uma Venda Normal (1) ou se vier vazio (0)
        if (dto.Tipo == TipoBaixa.Venda || (int)dto.Tipo == 0)
        {
            if (string.IsNullOrEmpty(dto.Comprador))
                throw new Exception("Informe o nome do comprador.");
                
            if (dto.PrecoArroba <= 0)
                throw new Exception("O valor da arroba negociada deve ser maior que zero.");
        }

        // 2. Cria o objeto Venda (Cabeçalho)
        var novaVenda = new Venda
        {
            Comprador = dto.Comprador,
            
            // AJUSTE 3: Gravamos o tipo no banco de dados
            Tipo = (int)dto.Tipo == 0 ? TipoBaixa.Venda : dto.Tipo, 
            
            DataVenda = dto.DataVenda == DateTime.MinValue ? DateTime.Now : dto.DataVenda,
            PrecoArroba = dto.PrecoArroba, 
            
            // --- CONFIANÇA NO FRONT-END ---
            // Pegamos o valor exato combinado na tela (R$ 5.000,00) para evitar dízima e centavos errados
            ValorTotal = dto.ValorTotal > 0 ? dto.ValorTotal : 0, 
            
            Ativo = true,
            Itens = new List<ItemVenda>()
        };

        // 3. Processa cada animal selecionado para a venda
        foreach (var itemDto in dto.Itens)
        {
            if (itemDto.PesoVivo <= 0)
                throw new Exception($"O peso vivo (na fazenda) para o animal ID {itemDto.AnimalId} é obrigatório.");

            // Busca o animal no banco
            var animal = await _animalRepositorio.ObterPorIdAsync(itemDto.AnimalId);

            if (animal == null)
                throw new Exception($"Animal ID {itemDto.AnimalId} não encontrado.");

            if (!animal.Ativo)
                throw new Exception($"Animal {animal.ID} já consta como vendido/inativo.");

            // --- SNAPSHOT (A FOTOGRAFIA DO PASSADO) ---
            var pesoEntradaOriginal = animal.Peso;
            var valorEntradaOriginal = animal.ValorEntrada;
            var racaOriginal = animal.Raca;

            // --- ATUALIZAÇÃO DO ANIMAL ---
            // Atualiza o peso e dá baixa no estoque (Tira do pasto)
            animal.Peso = itemDto.PesoVivo; 
            animal.Ativo = false;
            await _animalRepositorio.AtualizarAsync(animal);

            // --- CRIA O ITEM DA VENDA ---
            var novoItem = new ItemVenda
            {
                AnimalID = animal.ID,
                PesoVivo = itemDto.PesoVivo, 
                
                // Grava a fotografia no item da venda para manter o relatório eterno
                Raca = racaOriginal,
                PesoEntrada = pesoEntradaOriginal,
                ValorEntrada = valorEntradaOriginal
            };
            
            // Tenta processar caso o usuário já tenha o peso morto no ato da venda
            if(itemDto.PesoMorto.HasValue && itemDto.PesoMorto > 0)
            {
                novoItem.PesoMorto = itemDto.PesoMorto;
                novoItem.ProcessarRomaneio(novaVenda.PrecoArroba);
            }

            novaVenda.Itens.Add(novoItem);
        }
        
        // Removemos o CalcularTotalDaVenda() daqui para que o C# não substitua 
        // o ValorTotal exato que veio da tela por uma conta com dízimas.

        // 4. Salva tudo no banco
        return await _vendaRepositorio.SalvarAsync(novaVenda);
    }

    // --- MÉTODOS DE LEITURA E CANCELAMENTO CONTINUAM IGUAIS ---
    public async Task<Venda> ObterAsync(int id)
    {
        return await _vendaRepositorio.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<Venda>> ListarAsync(bool ativo, string queryComprador)
    {
        return await _vendaRepositorio.ListarAsync(ativo, queryComprador);
    }

    public async Task AtualizarRomaneioAsync(AtualizarVendaDTO dto)
    {
        var vendaBanco = await _vendaRepositorio.ObterPorIdAsync(dto.VendaID);
        if (vendaBanco == null) throw new Exception("Venda não encontrada.");
        if (!vendaBanco.Ativo) throw new Exception("Não é possível editar uma venda cancelada.");

        vendaBanco.Comprador = dto.Comprador;
        vendaBanco.PrecoArroba = dto.PrecoArroba;
        
        foreach(var itemVendaBanco in vendaBanco.Itens)
        {
            var itemRecebido = dto.Itens.FirstOrDefault(i => i.AnimalId == itemVendaBanco.AnimalID);
            
            if(itemRecebido != null && itemRecebido.PesoMorto.HasValue && itemRecebido.PesoMorto > 0)
            {
                itemVendaBanco.PesoMorto = itemRecebido.PesoMorto.Value;
                itemVendaBanco.ProcessarRomaneio(vendaBanco.PrecoArroba);
            }
        }
        
        vendaBanco.CalcularTotalDaVenda();
        await _vendaRepositorio.AtualizarAsync(vendaBanco);
    }

    public async Task ExcluirAsync(int id) 
    {
        var venda = await _vendaRepositorio.ObterPorIdAsync(id);

        if (venda == null) throw new Exception("Venda não encontrada.");

        if (venda.Itens != null && venda.Itens.Any())
        {
            foreach (var item in venda.Itens)
            {
                var animal = await _animalRepositorio.ObterPorIdAsync(item.AnimalID);
                if (animal != null)
                {
                    animal.Ativo = true; 
                    animal.Peso = item.PesoEntrada; 
                    await _animalRepositorio.AtualizarAsync(animal);
                }
            }
        }

        await _vendaRepositorio.DeletarAsync(venda);
    }
}