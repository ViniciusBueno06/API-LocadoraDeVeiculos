using API_LocadoraVeiculos.Models.Pagamentos;
using System.Text.Json.Serialization;

namespace API_LocadoraVeiculos.DTOs.PagamentosDto
{
    public class ConfirmarPagamentoRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MetodoPag MetodoPagamento { get; set; }
    }
}