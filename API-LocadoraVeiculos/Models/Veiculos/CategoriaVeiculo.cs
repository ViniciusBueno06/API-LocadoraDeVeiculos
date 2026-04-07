using System.Text.Json.Serialization;

namespace API_LocadoraVeiculos.Models.Veiculos
{
    public class CategoriaVeiculo
    {
        public int Id { get; set; } 
        public string NomeCateg { get; set; }  
        public string Descricao { get; set; }

        [JsonIgnore]
        public ICollection<VeiculoModel> Veiculos { get; set; }
    }
}
