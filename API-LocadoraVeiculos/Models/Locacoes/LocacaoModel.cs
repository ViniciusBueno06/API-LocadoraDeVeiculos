using API_LocadoraVeiculos.Models.Clientes;
using API_LocadoraVeiculos.Models.Veiculos;

namespace API_LocadoraVeiculos.Models.Locacoes
{
    public class LocacaoModel
    {
        public int Id { get; set; }
        public ClienteModel Cliente { get; set; }
        public VeiculoModel Veiculo { get; set; }
        public DateTime DataInicio { get; set; }    
        public DateTime DataFimPrevista { get; set; }
        public DateTime DataFimReal { get;set; }
        public decimal ValorTotal { get; set; }
        public StatusLocacao Status { get; set; }

        public DateTime created_up { get; set; }
        public DateTime updated_up { get; set; }
    }
}
