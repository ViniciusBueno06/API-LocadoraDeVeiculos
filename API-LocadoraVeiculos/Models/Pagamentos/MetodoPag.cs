using System.Text.Json.Serialization;

namespace API_LocadoraVeiculos.Models.Pagamentos
{
    public enum MetodoPag
    {
        Credito = 1,
        Debito = 2,
        Pix = 3,
        Boleto = 4
    }
}
