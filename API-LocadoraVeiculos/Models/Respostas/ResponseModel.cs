namespace API_LocadoraVeiculos.Models.Respostas
{
    public class ResponseModel<T>
    {
        public bool Sucesso { get; set; } = true;
        public string Mensagem { get; set; } = "";
        public T? Dados { get; set; }
        public List<string> Erros { get; set; } 
    }
}
