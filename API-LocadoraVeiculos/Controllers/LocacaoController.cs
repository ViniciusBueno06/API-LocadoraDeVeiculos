
using API_LocadoraVeiculos.DTOs.LocacoesDtos;
using API_LocadoraVeiculos.DTOs.PagamentosDto;
using API_LocadoraVeiculos.Models.Locacoes;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacaoController : ControllerBase
    {
        private readonly ILocacaoInterface _locacaoInterface;

        public LocacaoController(ILocacaoInterface locacaoInterface)
        {
            _locacaoInterface = locacaoInterface;
        }

        [HttpGet("ListarLocacoes")]
        public async Task<ActionResult<ResponseModel<List<LocacaoResponseDto>>>> ListarLocacoes()
        {
            var locacoes = await _locacaoInterface.ListarLocacoes();
            if (locacoes.Sucesso && locacoes.Dados == null) return NotFound(locacoes);
            if (!locacoes.Sucesso) return BadRequest(locacoes);
            return Ok(locacoes);
        }

        [HttpGet("BuscarLocacaoPorId/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<LocacaoResponseDto>>> BuscarLocacaoPorId(int idLocacao)
        {
            var locacao = await _locacaoInterface.BuscarLocacaoPorId(idLocacao);
            if (locacao.Dados == null) return NotFound(locacao);
            if (!locacao.Sucesso) return BadRequest(locacao);
            return Ok(locacao);
        }

        [HttpPost("CriarLocacao")]
        public async Task<ActionResult<ResponseModel<LocacaoResponseDto>>> CriarLocacao(LocacaoRequestDto dto)
        {
            var locacao = await _locacaoInterface.CriarLocacao(dto);
            if (locacao.Sucesso == true) return CreatedAtAction(nameof(BuscarLocacaoPorId), new { idLocacao = locacao.Dados.Id }, locacao);
            return BadRequest(locacao);
        }

        [HttpPost("ConfirmarPagamento/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<PagamentoResponseDto>>> ConfirmarPagamento(int idLocacao, [FromBody] ConfirmarPagamentoRequestDto request)
        {
            var pagamento = await _locacaoInterface.ConfirmarPagamento(idLocacao, request);
            if (pagamento.Sucesso == true && pagamento.Dados != null) return Ok(pagamento);
            if (pagamento.Dados == null) return NotFound(pagamento);
            return BadRequest(pagamento);
        }

        [HttpPut("EditarLocacao/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<LocacaoResponseDto>>> EditarLocacao(int idLocacao, LocacaoUpdateDto dto)
        {
            var locacao = await _locacaoInterface.EditarLocacao(idLocacao, dto);
            if (locacao.Sucesso == true) return Ok(locacao);
            return BadRequest(locacao);
        }

        [HttpPatch("EncerrarLocacao/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<LocacaoResponseDto>>> EncerrarLocacao(int idLocacao)
        {
            var locacao = await _locacaoInterface.EncerrarLocacao(idLocacao);
            if (locacao.Sucesso == true) return Ok(locacao);
            return BadRequest(locacao);
        }

        [HttpDelete("ExcluirLocacao/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<LocacaoResponseDto>>> ExcluirLocacao(int idLocacao)
        {
            var locacao = await _locacaoInterface.ExcluirLocacao(idLocacao);
            if (locacao.Sucesso == true) return Ok(locacao);
            return BadRequest(locacao);
        }


    }
}