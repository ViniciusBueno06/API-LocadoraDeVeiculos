namespace API_LocadoraVeiculos.DTOs.VeiculosDto
{
    public class VeiculoRequestDto
    {
        public string NomeModelo { get; set; }
        public string Placa { get; set; }
        public int CategoriaId { get; set; }
        public int CorId { get; set; }
        public int AnoVeiculo { get; set; }
        public decimal ValorDiaria { get; set; }
    }
}