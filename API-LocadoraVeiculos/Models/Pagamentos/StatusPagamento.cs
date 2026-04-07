using System.Text.Json.Serialization;

namespace API_LocadoraVeiculos.Models.Pagamentos
{
    public enum StatusPagamento
    {
        Pendente = 1,
        Realizado = 2,
        Cancelado = 3
    }
}