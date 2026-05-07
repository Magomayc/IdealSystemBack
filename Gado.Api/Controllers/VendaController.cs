using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gado.Aplicacao; 
using Gado.Aplicacao.DTOs; 
using Gado.Api.Models.Requisicao; 
using Gado.Api.Models.Resposta;   
using Gado.Dominio.Enumeradores; // Adicionei a referência do enumerador

namespace Gado.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendaController : ControllerBase
{
    private readonly IVendaAplicacao _vendaApp;

    public VendaController(IVendaAplicacao vendaApp)
    {
        _vendaApp = vendaApp;
    }

    [HttpPost]
    [Route("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] VendaCriar model)
    {
        try
        {
            var dto = new RegistrarVendaDTO
            {
                // NOVO: Passando o tipo que veio do React (1 = Venda, 2 = Baixa)
                Tipo = model.Tipo, 
                
                Comprador = model.Comprador,
                DataVenda = model.DataVenda,
                PrecoArroba = model.PrecoArroba,
                ValorTotal = model.ValorTotal, 
                
                Itens = model.Itens.Select(i => new ItemVendaDTO
                {
                    AnimalId = i.AnimalId,
                    PesoVivo = i.PesoVivo,
                    PesoMorto = i.PesoMorto,
                    Raca = i.Raca,
                    PesoEntrada = i.PesoEntrada,
                    ValorEntrada = i.ValorEntrada
                }).ToList()
            };

            var vendaCriada = await _vendaApp.CriarAsync(dto);
            return Ok(new { mensagem = "Operação registrada com sucesso!", id = vendaCriada.ID });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPut]
    [Route("AtualizarRomaneio")]
    public async Task<IActionResult> AtualizarRomaneio([FromBody] VendaAtualizar model)
    {
        try
        {
            var dto = new AtualizarVendaDTO
            {
                VendaID = model.ID,
                Comprador = model.Comprador,
                PrecoArroba = model.PrecoArroba,
                Itens = model.Itens.Select(i => new ItemAtualizarDTO
                {
                    AnimalId = i.AnimalId,
                    PesoMorto = i.PesoMorto
                }).ToList()
            };

            await _vendaApp.AtualizarRomaneioAsync(dto);
            return Ok(new { mensagem = "Romaneio do frigorífico atualizado e valores recalculados com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet]
    [Route("Listar")]
    public async Task<IActionResult> Listar([FromQuery] bool ativo = true, [FromQuery] string query = "")
    {
        try 
        {
            var listaBanco = await _vendaApp.ListarAsync(ativo, query);
            
            var listaResposta = listaBanco.Select(v => new VendaResposta
            {
                Id = v.ID,
                
                // NOVO: Devolvendo o tipo para o React saber se é Venda ou Baixa
                Tipo = v.Tipo, 
                
                Comprador = v.Comprador,
                DataVenda = v.DataVenda,
                PrecoArroba = v.PrecoArroba,
                ValorTotal = v.ValorTotal,
                Ativo = v.Ativo,
                Itens = v.Itens.Select(i => new ItemVendaResposta
                {
                    AnimalID = i.AnimalID,
                    PesoVivo = i.PesoVivo,
                    PesoMorto = i.PesoMorto,
                    RendimentoCarcaca = i.RendimentoCarcaca,
                    TotalArrobas = i.TotalArrobas,
                    ValorUnitario = i.ValorUnitario,
                    Raca = i.Raca,
                    PesoEntrada = i.PesoEntrada,
                    ValorEntrada = i.ValorEntrada,
                    
                    Animal = i.Animal == null ? null : new AnimalResposta 
                    { 
                        Id = i.Animal.ID, 
                        Brinco = i.Animal.Brinco 
                    }
                }).ToList()
            });

            return Ok(listaResposta);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpDelete]
    [Route("Excluir/{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        try
        {
            await _vendaApp.ExcluirAsync(id);
            return Ok(new { mensagem = "Operação cancelada com sucesso. Os animais retornaram ao estoque com o peso original." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}