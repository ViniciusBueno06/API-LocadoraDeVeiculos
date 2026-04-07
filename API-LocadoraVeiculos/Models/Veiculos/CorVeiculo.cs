namespace API_LocadoraVeiculos.Models.Veiculos
{
    public class CorVeiculo
    {
        public int Id { get; set; }
        public string cor { get; set; }
        public string HexaCor { get; set; } 
        
        public ICollection<VeiculoModel> Veiculos { get; set; }    
    }
}