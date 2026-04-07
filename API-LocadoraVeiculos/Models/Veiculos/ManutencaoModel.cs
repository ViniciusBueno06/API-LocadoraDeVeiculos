namespace API_LocadoraVeiculos.Models.Veiculos
{
    public class ManutencaoModel
    {
        public int Id { get; set; }
        public VeiculoModel VeiculoId { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
