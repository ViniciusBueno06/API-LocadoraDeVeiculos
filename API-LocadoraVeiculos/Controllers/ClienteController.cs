using API_LocadoraVeiculos.DTOs.Clientes;
using API_LocadoraVeiculos.DTOs.ClientesDtos;
using API_LocadoraVeiculos.Models.Clientes;
using API_LocadoraVeiculos.Models.Respostas;
using API_LocadoraVeiculos.Services.Interfaces;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LocadoraVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteInterface _clienteInterface;
        public ClienteController(IClienteInterface clienteInterface)
        {
            _clienteInterface = clienteInterface;
        }

        [HttpGet("ListarClientes")]
        public async Task<ActionResult<ResponseModel<List<ClienteResponseDto>>>> ListarClientes()
        {
            
            var clientes = await _clienteInterface.ListarClientes();

            if(clientes.Sucesso && clientes.Dados == null) return NotFound(clientes);

            if(!clientes.Sucesso) return BadRequest(clientes);
           
            return Ok(clientes);

        }

        [HttpGet("BuscarClientePorId/{idCliente}")]
        public async Task<ActionResult<ResponseModel<ClienteModel>>> BuscarClientePorId(int idCliente)
        {
            var cliente = await _clienteInterface.BuscaClientePorId(idCliente);
            if(cliente.Dados==null) return NotFound(cliente);

            if (!cliente.Sucesso) return BadRequest(cliente);

            return Ok(cliente);
        }

        [HttpGet("BuscarClientePorIdLocacao/{idLocacao}")]
        public async Task<ActionResult<ResponseModel<ClienteModel>>> BuscarClientePorIdLocacao(int idLocacao)
        {
            var cliente = await _clienteInterface.BuscarClientePorIdLocacao(idLocacao);
            return Ok(cliente);
        }

        [HttpPost("CriarCliente")]
        public async Task<ActionResult<ResponseModel<ClienteResponseDto>>> CriarCliente(ClienteRequestDto dto)
        {
            var cliente = await _clienteInterface.CriarCliente(dto);
            if(cliente.Sucesso == true) return CreatedAtAction(nameof(BuscarClientePorId), new { idCliente = cliente.Dados.Id }, cliente);
            return BadRequest(cliente);
        }

        [HttpPut("EditarCliente/{idCliente}")]
        public async Task<ActionResult<ResponseModel<ClienteModel>>> EditarCliente(int idCliente, ClienteUpdateDto dto)
        {
            var cliente = await _clienteInterface.EditarCliente(idCliente,dto);
            if (cliente.Sucesso == true) return Ok(cliente);
            return BadRequest(cliente);


        }

        [HttpDelete("ExcluirCliente/{idCliente}")]
        public async Task<ActionResult<ResponseModel<ClienteResponseDto>>> ExcluirCliente (int idCliente)
        {
            var cliente = await _clienteInterface.ExcluirCliente(idCliente);
            if (cliente.Sucesso == true) return Ok(cliente);
            return BadRequest(cliente);
        }


    }
}
