using API_LocadoraVeiculos.DTOs.VeiculosDto;
using API_LocadoraVeiculos.Models.Respostas;

namespace API_LocadoraVeiculos.Services.Interfaces
{
    public interface IVeiculoInterface
    {
        Task<ResponseModel<List<VeiculoResponseDto>>> ListarVeiculos();
        Task<ResponseModel<VeiculoResponseDto>> BuscarVeiculoPorId(int idVeiculo);
        Task<ResponseModel<VeiculoResponseDto>> AdicionarVeiculo(VeiculoRequestDto dto);
        Task<ResponseModel<VeiculoResponseDto>> EditarVeiculo(int idVeiculo, VeiculoUpdateDto dto);
        Task<ResponseModel<VeiculoResponseDto>> RemoverVeiculo(int idVeiculo);
    }
}