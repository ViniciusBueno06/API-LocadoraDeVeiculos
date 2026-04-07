using API_LocadoraVeiculos.DTOs.VeiculosDto;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {
        private readonly IVeiculoInterface _veiculoInterface;

        public VeiculoController(IVeiculoInterface veiculoInterface)
        {
            _veiculoInterface = veiculoInterface;
        }

        [HttpGet("ListarVeiculos")]
        public async Task<ActionResult<ResponseModel<List<VeiculoResponseDto>>>> ListarVeiculos()
        {
            var veiculos = await _veiculoInterface.ListarVeiculos();
            if (veiculos.Sucesso && veiculos.Dados == null) return NotFound(veiculos);
            if (!veiculos.Sucesso) return BadRequest(veiculos);
            return Ok(veiculos);
        }

        [HttpGet("BuscarVeiculoPorId/{idVeiculo}")]
        public async Task<ActionResult<ResponseModel<VeiculoResponseDto>>> BuscarVeiculoPorId(int idVeiculo)
        {
            var veiculo = await _veiculoInterface.BuscarVeiculoPorId(idVeiculo);
            if (veiculo.Dados == null) return NotFound(veiculo);
            if (!veiculo.Sucesso) return BadRequest(veiculo);
            return Ok(veiculo);
        }

        [HttpPost("AdicionarVeiculo")]
        public async Task<ActionResult<ResponseModel<VeiculoResponseDto>>> AdicionarVeiculo(VeiculoRequestDto dto)
        {
            var veiculo = await _veiculoInterface.AdicionarVeiculo(dto);
            if (veiculo.Sucesso == true) return CreatedAtAction(nameof(BuscarVeiculoPorId), new { idVeiculo = veiculo.Dados.Id }, veiculo);
            return BadRequest(veiculo);
        }

        [HttpPut("EditarVeiculo/{idVeiculo}")]
        public async Task<ActionResult<ResponseModel<VeiculoResponseDto>>> EditarVeiculo(int idVeiculo, VeiculoUpdateDto dto)
        {
            var veiculo = await _veiculoInterface.EditarVeiculo(idVeiculo, dto);
            if (veiculo.Sucesso == true) return Ok(veiculo);
            return BadRequest(veiculo);
        }

        [HttpDelete("RemoverVeiculo/{idVeiculo}")]
        public async Task<ActionResult<ResponseModel<VeiculoResponseDto>>> RemoverVeiculo(int idVeiculo)
        {
            var veiculo = await _veiculoInterface.RemoverVeiculo(idVeiculo);
            if (veiculo.Sucesso == true) return Ok(veiculo);
            return BadRequest(veiculo);
        }
    }
}