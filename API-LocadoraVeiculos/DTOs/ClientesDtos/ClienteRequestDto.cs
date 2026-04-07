using System.ComponentModel.DataAnnotations;

namespace API_LocadoraVeiculos.DTOs.Clientes
{
    public class ClienteRequestDto
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 caracteres")]
        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        public DateTime DataNasc { get; set; }

        [Required]
        public string NumCnh { get; set; }

        [Required]
        public DateTime ValidadeCnh { get; set; }
    }
}