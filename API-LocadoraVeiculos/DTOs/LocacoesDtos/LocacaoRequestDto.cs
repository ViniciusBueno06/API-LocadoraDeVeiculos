using API_LocadoraVeiculos.Models.Pagamentos;

public class LocacaoRequestDto
{
    public int ClienteId { get; set; }
    public int VeiculoId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public MetodoPag MetodoPag { get; set; } 
}