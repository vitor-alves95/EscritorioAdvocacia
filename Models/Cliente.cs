using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscritorioAdvocacia.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome Completo' é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'CPF' é obrigatório.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 ou 14 caracteres.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido (ex: nome@dominio.com).")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O 'Telefone' é obrigatório.")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<Processo> Processos { get; set; }
    }
}