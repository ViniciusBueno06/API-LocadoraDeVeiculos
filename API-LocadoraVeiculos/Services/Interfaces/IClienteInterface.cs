using API_LocadoraVeiculos.DTOs.Clientes;
using API_LocadoraVeiculos.DTOs.ClientesDtos;
using API_LocadoraVeiculos.Models.Clientes;
using API_LocadoraVeiculos.Models.Respostas;

namespace API_LocadoraVeiculos.Services.Interfaces
{
    public interface IClienteInterface
    {
        Task<ResponseModel<List<ClienteResponseDto>>> ListarClientes();
        Task<ResponseModel<ClienteResponseDto>> BuscaClientePorId(int IdCLiente);
        Task<ResponseModel<ClienteResponseDto>> BuscarClientePorIdLocacao (int IdLocacao);
        Task<ResponseModel<ClienteResponseDto>> CriarCliente(ClienteRequestDto cliente);
        Task<ResponseModel<ClienteResponseDto>> ExcluirCliente(int idCliente);
        Task<ResponseModel<ClienteModel>> EditarCliente(int idCliente, ClienteUpdateDto dto);
    }
}
