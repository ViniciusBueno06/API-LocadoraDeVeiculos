using API_LocadoraVeiculos.Models.Veiculos;

namespace API_LocadoraVeiculos.DTOs.VeiculosDto
{
    public class VeiculoUpdateDto
    {
        public string NomeModelo { get; set; }
        public string Placa { get; set; }
        public int CategoriaId { get; set; }
        public int CorId { get; set; }
        public int AnoVeiculo { get; set; }
        public decimal ValorDiaria { get; set; }
        public StatusVeiculo Status { get; set; }
    }
}