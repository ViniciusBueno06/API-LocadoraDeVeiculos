using API_LocadoraVeiculos.Models.Locacoes;

namespace API_LocadoraVeiculos.Models.Pagamentos
{
    public class PagamentoModel
    {
        public int Id { get; set; } 
        public LocacaoModel Locacao { get; set; }   
        public decimal Valor { get; set; }   
        public MetodoPag MetodoPag { get; set; }    
        public StatusPagamento Status { get; set; }

        public DateTime Data_pagamento { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
