using Microsoft.VisualBasic;

namespace API_LocadoraVeiculos.Models.Clientes
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf {get;set; }   
        public string Email { get; set; }   
        public string Telefone { get; set; }    
        public DateTime DataNasc {  get; set; }   
        public string NumCnh { get; set;}
        public DateTime ValidadeCnh {  get; set; }   
        
        public bool Status {  get; set; }   
        public DateTime created_at { get; set; }    
        public DateTime updated_at { get; set; }

    }
}
