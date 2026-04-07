namespace API_LocadoraVeiculos.Models.Veiculos
{
    public class VeiculoModel
    {
        public int Id { get; set; } 
        public string NomeModelo {get; set; }
        public string Placa {  get; set; }

        public int CategoriaId { get; set; }
        public CategoriaVeiculo Categoria { get; set; }

        public int CorId { get; set; }
        public CorVeiculo Cor { get; set; }

        public int AnoVeiculo { get; set;}
        public decimal ValorDiaria { get; set; }
        public StatusVeiculo Status { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
