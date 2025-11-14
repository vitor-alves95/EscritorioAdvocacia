using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscritorioAdvocacia.Models
{
    public class Advogado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Nome do Advogado")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo 'OAB' é obrigatório.")]
        [Display(Name = "Nº da OAB")]
        public string OAB { get; set; }

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public string Email { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<Processo> Processos { get; set; }
    }
}