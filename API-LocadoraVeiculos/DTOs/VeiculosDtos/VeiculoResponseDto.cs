using API_LocadoraVeiculos.Models.Veiculos;

namespace API_LocadoraVeiculos.DTOs.VeiculosDto
{
    public class VeiculoResponseDto
    {
        public int Id { get; set; }
        public string NomeModelo { get; set; }
        public string Placa { get; set; }
        public string CategoriaNome { get; set; }
        public string CorNome { get; set; }
        public string CorHexa { get; set; }
        public int AnoVeiculo { get; set; }
        public decimal ValorDiaria { get; set; }
        public string Status { get; set; }
    }
}