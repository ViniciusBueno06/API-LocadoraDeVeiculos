using API_LocadoraVeiculos.DTOs.LocacoesDtos;
using API_LocadoraVeiculos.DTOs.PagamentosDto;
using API_LocadoraVeiculos.Models.Respostas;

namespace API_LocadoraVeiculos.Services.Interfaces
{
    public interface ILocacaoInterface
    {
        Task<ResponseModel<List<LocacaoResponseDto>>> ListarLocacoes();
        Task<ResponseModel<LocacaoResponseDto>> BuscarLocacaoPorId(int idLocacao);
        Task<ResponseModel<LocacaoResponseDto>> CriarLocacao(LocacaoRequestDto dto);
        Task<ResponseModel<LocacaoResponseDto>> EditarLocacao(int idLocacao, LocacaoUpdateDto dto);
        Task<ResponseModel<LocacaoResponseDto>> EncerrarLocacao(int idLocacao);
        Task<ResponseModel<LocacaoResponseDto>> ExcluirLocacao(int idLocacao);

        Task<ResponseModel<PagamentoResponseDto>> ConfirmarPagamento(int idLocacao, ConfirmarPagamentoRequestDto request);
    }
}
