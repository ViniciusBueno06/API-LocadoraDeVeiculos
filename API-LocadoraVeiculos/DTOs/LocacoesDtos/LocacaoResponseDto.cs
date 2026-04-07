using API_LocadoraVeiculos.Models.Locacoes;

public class LocacaoResponseDto
{
    public int Id { get; set; }
    public string NomeCliente { get; set; }
    public string NomeVeiculo { get; set; }
    public string Placa { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime? DataFimReal { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusLocacao Status { get; set; }
}