using Microsoft.AspNetCore.Mvc;
using Gado.Dominio.Entidades;
using Gado.Aplicacao;
using Gado.Api.Models.Requisicao;
using Gado.Api.Models.Resposta;

namespace Gado.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MovimentacaoMilhoController : ControllerBase
    {
        private readonly IMovimentacaoMilhoAplicacao _movimentacaoAplicacao;

        public MovimentacaoMilhoController(IMovimentacaoMilhoAplicacao movimentacaoAplicacao)
        {
            _movimentacaoAplicacao = movimentacaoAplicacao;
        }

        [HttpPost]
        [Route("Criar")]
        public async Task<IActionResult> CriarAsync([FromBody] MovimentacaoMilhoCriar requisicao)
        {
            try
            {
                var movimentacao = new MovimentacaoMilho
                {
                    MilhoID = requisicao.MilhoID,
                    Tipo = requisicao.Tipo, 
                    QuantidadeKg = requisicao.QuantidadeKg,
                    DataMovimentacao = requisicao.DataMovimentacao,
                    
                    // --- MAPEAMENTO DOS CAMPOS DE VENDA ---
                    ValorVenda = requisicao.ValorVenda,
                    ValorPorSacoVendido = requisicao.ValorPorSacoVendido, 
                    Pagamento = requisicao.Pagamento,                    
                    Comprador = requisicao.Comprador,
                    
                    Observacao = requisicao.Observacao
                };

                var id = await _movimentacaoAplicacao.CriarAsync(movimentacao);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarAsync([FromBody] MovimentacaoMilhoAtualizar requisicao)
        {
            try
            {
                var movimentacao = new MovimentacaoMilho
                {
                    ID = requisicao.ID,
                    Tipo = requisicao.Tipo,
                    QuantidadeKg = requisicao.QuantidadeKg,
                    DataMovimentacao = requisicao.DataMovimentacao,
                    
                    // --- MAPEAMENTO DOS CAMPOS DE VENDA ---
                    ValorVenda = requisicao.ValorVenda,
                    ValorPorSacoVendido = requisicao.ValorPorSacoVendido, 
                    Pagamento = requisicao.Pagamento,                    
                    Comprador = requisicao.Comprador,
                    
                    Observacao = requisicao.Observacao
                };

                await _movimentacaoAplicacao.AtualizarAsync(movimentacao);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obter/{id}")]
        public async Task<IActionResult> ObterAsync([FromRoute] int id)
        {
            try
            {
                var movimentacao = await _movimentacaoAplicacao.ObterAsync(id);
                var resposta = ConverterParaResposta(movimentacao);
                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Route("Listar")]
        public async Task<IActionResult> ListarAsync([FromQuery] bool ativo = true, [FromQuery] int? milhoId = null)
        {
            try
            {
                var lista = await _movimentacaoAplicacao.ListarAsync(ativo, milhoId);
                var listaResposta = lista.Select(m => ConverterParaResposta(m));
                return Ok(listaResposta);
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
                // Quando o React chama isso, a Aplicação vai primeiro devolver o milho pro Silo!
                await _movimentacaoAplicacao.DeletarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("Restaurar/{id}")]
        public async Task<IActionResult> RestaurarAsync([FromRoute] int id)
        {
            try
            {
                await _movimentacaoAplicacao.RestaurarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        private static MovimentacaoMilhoResposta ConverterParaResposta(MovimentacaoMilho movimentacao)
        {
            return new MovimentacaoMilhoResposta
            {
                Id = movimentacao.ID,
                MilhoId = movimentacao.MilhoID,
                Tipo = movimentacao.Tipo,
                QuantidadeKg = movimentacao.QuantidadeKg,
                DataMovimentacao = movimentacao.DataMovimentacao,
                
                // --- MAPEAMENTO DOS CAMPOS DE VENDA ---
                ValorVenda = movimentacao.ValorVenda,
                ValorPorSacoVendido = movimentacao.ValorPorSacoVendido, 
                Pagamento = movimentacao.Pagamento,                    
                Comprador = movimentacao.Comprador,
                
                CustoMovimentacao = movimentacao.CustoMovimentacao,
                Observacao = movimentacao.Observacao,
                Ativo = movimentacao.Ativo
            };
        }
    }
}