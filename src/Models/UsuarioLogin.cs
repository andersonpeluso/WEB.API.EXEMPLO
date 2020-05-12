using System.ComponentModel.DataAnnotations;

namespace Modelo.API.Models
{
    public class UsuarioLogin
    {
        public int ID { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public bool Status { get; set; }

        public bool AtivaLog { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        public Token Acesso { get; set; }
    }
}