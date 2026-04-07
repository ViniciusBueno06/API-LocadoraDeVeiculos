using System.ComponentModel.DataAnnotations;

namespace API_LocadoraVeiculos.DTOs.ClientesDtos
{
    public class ClienteUpdateDto
    {
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
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
