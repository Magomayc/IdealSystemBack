using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gado.Aplicacao;
using Gado.Aplicacao.DTOs;
using Gado.Api.Models.Requisicao;
using Gado.Api.Models.Resposta;

namespace Gado.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaixaAnimalController : ControllerBase
{
    private readonly IBaixaAnimalAplicacao _baixaApp;

    public BaixaAnimalController(IBaixaAnimalAplicacao baixaApp)
    {
        _baixaApp = baixaApp;
    }

    [HttpPost]
    [Route("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] BaixaAnimalCriar model)
    {
        try
        {
            // Mapeia da API para o DTO da Aplicação
            var dto = new RegistrarBaixaDTO
            {
                AnimalID = model.AnimalID,
                DataBaixa = model.DataBaixa,
                Motivo = model.Motivo,
                Observacao = model.Observacao
            };

            var baixaCriada = await _baixaApp.CriarAsync(dto);
            return Ok(new { mensagem = "Baixa registrada com sucesso. O animal foi retirado do estoque.", id = baixaCriada.ID });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPut]
    [Route("Atualizar")]
    public async Task<IActionResult> Atualizar([FromBody] BaixaAnimalAtualizar model)
    {
        try
        {
            var dto = new AtualizarBaixaDTO
            {
                BaixaID = model.ID,
                DataBaixa = model.DataBaixa,
                Motivo = model.Motivo,
                Observacao = model.Observacao
            };

            await _baixaApp.AtualizarAsync(dto);
            return Ok(new { mensagem = "Registro de baixa atualizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet]
    [Route("Listar")]
    public async Task<IActionResult> Listar([FromQuery] bool ativo = true)
    {
        try
        {
            var listaBanco = await _baixaApp.ListarAsync(ativo);

            // Mapeia do Banco para a Resposta da API
            var listaResposta = listaBanco.Select(b => new BaixaAnimalResposta
            {
                Id = b.ID,
                DataBaixa = b.DataBaixa,
                Motivo = b.Motivo,
                Observacao = b.Observacao,
                Ativo = b.Ativo,
                AnimalID = b.AnimalID,
                Animal = b.Animal == null ? null : new AnimalResposta
                {
                    Id = b.Animal.ID,
                    Brinco = b.Animal.Brinco
                    // Adicione outros campos de AnimalResposta aqui, se precisar
                }
            }).ToList();

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
            await _baixaApp.ExcluirAsync(id);
            return Ok(new { mensagem = "Baixa cancelada com sucesso. O animal retornou ao estoque ativo!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}