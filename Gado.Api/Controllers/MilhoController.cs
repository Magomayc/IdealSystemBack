using Microsoft.AspNetCore.Mvc;
using Gado.Dominio.Entidades;
using Gado.Aplicacao;
using Gado.Api.Models.Requisicao;
using Gado.Api.Models.Resposta;

namespace Gado.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MilhoController : ControllerBase
    {
        private readonly IMilhoAplicacao _milhoAplicacao;

        public MilhoController(IMilhoAplicacao milhoAplicacao)
        {
            _milhoAplicacao = milhoAplicacao;
        }

        [HttpPost]
        [Route("Criar")]
        public async Task<IActionResult> CriarAsync([FromBody] MilhoCriar requisicao)
        {
            try
            {
                var milho = new Milho
                {
                    Vendedor = requisicao.Vendedor,
                    QuantidadeSacos = requisicao.QuantidadeSacos,
                    PesoPorSaco = requisicao.PesoPorSaco, 
                    ValorPorSaco = requisicao.ValorPorSaco,
                    Pagamento = requisicao.Pagamento,
                    DataCompra = requisicao.DataCompra
                };

                // 👇 AJUSTE 1: Chama a matemática do seu Domínio antes de salvar.
                // Sem isso, KgComprado, ValorTotal e Estoque vão ficar zerados no banco!
                milho.CalcularTotais();

                var id = await _milhoAplicacao.CriarAsync(milho);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensagem = ex.Message });
            }
        }

        [HttpPut]
        [Route("Atualizar")]
        public async Task<IActionResult> AtualizarAsync([FromBody] MilhoAtualizar requisicao)
        {
            try
            {
                var milho = new Milho
                {
                    ID = requisicao.ID,
                    Vendedor = requisicao.Vendedor,
                    QuantidadeSacos = requisicao.QuantidadeSacos,
                    PesoPorSaco = requisicao.PesoPorSaco,
                    ValorPorSaco = requisicao.ValorPorSaco,
                    Pagamento = requisicao.Pagamento,
                    DataCompra = requisicao.DataCompra
                };

                // 👇 AJUSTE 2: Recalcula se o cara editar a quantidade de sacos lá no Front-end.
                milho.CalcularTotais();

                await _milhoAplicacao.AtualizarAsync(milho);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Obter/{id}")]
        public async Task<IActionResult> ObterAsync([FromRoute] int id)
        {
            try
            {
                var milho = await _milhoAplicacao.ObterAsync(id);

                var resposta = ConverterParaResposta(milho);

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Listar")]
        // 👇 AJUSTE 3: Coloquei "bool ativo = true" para ele não quebrar se o Front não mandar nada na URL.
        public async Task<IActionResult> ListarAsync([FromQuery] bool ativo = true, [FromQuery] string query = "")
        {
            try
            {
                var lista = await _milhoAplicacao.ListarAsync(ativo, query ?? "");

                var listaResposta = lista.Select(m => ConverterParaResposta(m));

                return Ok(listaResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Deletar/{id}")]
        public async Task<IActionResult> DeletarAsync([FromRoute] int id)
        {
            try
            {
                await _milhoAplicacao.DeletarAsync(id);
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
                await _milhoAplicacao.RestaurarAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // --- MÉTODO PRIVADO PARA EVITAR CÓDIGO REPETIDO ---
        private static MilhoResposta ConverterParaResposta(Milho milho)
        {
            return new MilhoResposta
            {
                Id = milho.ID,
                DataCompra = milho.DataCompra,
                Vendedor = milho.Vendedor,
                KgComprado = milho.KgComprado,
                QuantidadeSacos = milho.QuantidadeSacos,
                PesoPorSaco = milho.PesoPorSaco,
                ValorPorSaco = milho.ValorPorSaco,
                ValorTotal = milho.ValorTotal,
                Pagamento = milho.Pagamento,
                KgEstoqueAtual = milho.KgEstoqueAtual,
                Ativo = milho.Ativo
            };
        }
    }
}