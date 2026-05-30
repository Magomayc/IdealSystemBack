using Microsoft.AspNetCore.Mvc;
using Gado.Dominio.Entidades;
using Gado.Aplicacao;
using Gado.Api.Models.Requisicao;
using Gado.Api.Models.Resposta;
using Gado.Dominio.Enumeradores;

namespace Gado.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalAplicacao _animalAplicacao;

        public AnimalController(IAnimalAplicacao animalAplicacao)
        {
            _animalAplicacao = animalAplicacao;
        }

        [HttpGet]
        [Route("Obter/{id}")]
        public async Task<IActionResult> ObterAsync([FromRoute] int id)
        {
            try
            {
                var animal = await _animalAplicacao.ObterAsync(id);

                var resposta = new AnimalResposta
                {
                    Id = animal.ID,
                    Peso = animal.Peso,
                    ValorEntrada = animal.ValorEntrada,
                    Vendedor = animal.Vendedor,
                    DataEntrada = animal.DataEntrada,
                    Ativo = animal.Ativo,
                    Sexo = animal.Sexo,
                    Brinco = animal.Brinco,
                    Raca = animal.Raca,
                    PrecoArroba = animal.PrecoArroba,
                    Estoque = animal.Estoque
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Criar")]
        public async Task<IActionResult> CriarAsync([FromBody] AnimalCriar requisicao)
        {
            try
            {
                // --- AJUSTE: VALIDAÇÃO DE BRINCO ÚNICO ---
                // Busca em ativos e inativos para varrer todo o histórico
                var ativos = await _animalAplicacao.ListarAsync(true, requisicao.Brinco, null);
                var inativos = await _animalAplicacao.ListarAsync(false, requisicao.Brinco, null);
                var historicoCompleto = ativos.Concat(inativos);

                if (historicoCompleto.Any(a => a.Brinco.Trim().ToLower() == requisicao.Brinco.Trim().ToLower()))
                {
                    return BadRequest(new { mensagem = $"O brinco #{requisicao.Brinco} já foi utilizado no sistema (em um animal ativo ou já baixado/vendido)." });
                }
                // -----------------------------------------

                var animal = new Animal
                {
                    Peso = requisicao.Peso,
                    ValorEntrada = requisicao.ValorEntrada,
                    Vendedor = requisicao.Vendedor,
                    DataEntrada = requisicao.DataEntrada,
                    Sexo = requisicao.Sexo,
                    Brinco = requisicao.Brinco,
                    Raca = requisicao.Raca,
                    PrecoArroba = requisicao.PrecoArroba,
                    Estoque = requisicao.Estoque
                };

                var id = await _animalAplicacao.CriarAsync(animal);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarAsync([FromBody] AnimalAtualizar requisicao)
        {
            try
            {
                // --- AJUSTE: IMPEDIR DUPLICIDADE NA EDIÇÃO ---
                var ativos = await _animalAplicacao.ListarAsync(true, requisicao.Brinco, null);
                var inativos = await _animalAplicacao.ListarAsync(false, requisicao.Brinco, null);
                var historicoCompleto = ativos.Concat(inativos);

                // Verifica se o brinco existe em OUTRO animal (ID diferente)
                if (historicoCompleto.Any(a => a.Brinco.Trim().ToLower() == requisicao.Brinco.Trim().ToLower() && a.ID != requisicao.ID))
                {
                    return BadRequest(new { mensagem = $"O brinco #{requisicao.Brinco} já pertence a outro animal no histórico do sistema." });
                }
                // ---------------------------------------------

                var animal = new Animal
                {
                    ID = requisicao.ID,
                    Peso = requisicao.Peso,
                    ValorEntrada = requisicao.ValorEntrada,
                    Vendedor = requisicao.Vendedor,
                    DataEntrada = requisicao.DataEntrada,
                    Sexo = requisicao.Sexo,
                    Brinco = requisicao.Brinco,
                    Raca = requisicao.Raca,
                    PrecoArroba = requisicao.PrecoArroba,
                    Estoque = requisicao.Estoque
                };

                await _animalAplicacao.AtualizarAsync(animal);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Deletar/{id}")]
        public async Task<IActionResult> DeletarAsync([FromRoute] int id)
        {
            try
            {
                await _animalAplicacao.DeletarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Restaurar/{id}")]
        public async Task<IActionResult> RestaurarAsync([FromRoute] int id)
        {
            try
            {
                await _animalAplicacao.RestaurarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarAsync([FromQuery] bool ativo = true, [FromQuery] string query = "", [FromQuery] TipoEstoque? estoque = null)
        {
            try
            {
                var lista = await _animalAplicacao.ListarAsync(ativo, query ?? "", estoque);

                var listaResposta = lista.Select(a => new AnimalResposta
                {
                    Id = a.ID,
                    Peso = a.Peso,
                    ValorEntrada = a.ValorEntrada,
                    Vendedor = a.Vendedor,
                    DataEntrada = a.DataEntrada,
                    Ativo = a.Ativo,
                    Sexo = a.Sexo,
                    Brinco = a.Brinco,
                    Raca = a.Raca,
                    PrecoArroba = a.PrecoArroba,
                    Estoque = a.Estoque 
                });

                return Ok(listaResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}