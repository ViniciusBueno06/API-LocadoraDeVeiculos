using API_LocadoraVeiculos.Models.Pagamentos;

namespace API_LocadoraVeiculos.DTOs.PagamentosDto
{
    public class PagamentoResponseDto
    {
        public int Id { get; set; }
        public int LocacaoId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; }
        public string Status { get; set; }
        public DateTime DataPagamento { get; set; }
        public string MensagemConfirmacao { get; set; }
    }
}