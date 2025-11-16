using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscritorioAdvocacia.Models
{
    public class Compromisso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O 'Título' do compromisso é obrigatório.")]
        [StringLength(100)]
        public string Titulo { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A 'Data/Hora de Início' é obrigatória.")]
        [Display(Name = "Início")]
        public DateTime DataHoraInicio { get; set; }

        [Required(ErrorMessage = "A 'Data/Hora de Fim' é obrigatória.")]
        [Display(Name = "Fim")]
        public DateTime DataHoraFim { get; set; }

        [StringLength(200)]
        public string Local { get; set; }

        [Required]
        [Display(Name = "Advogado")]
        public int AdvogadoId { get; set; }
        [ForeignKey("AdvogadoId")]
        public virtual Advogado Advogado { get; set; }

        [Display(Name = "Processo Associado")]
        public int? ProcessoId { get; set; }
        [ForeignKey("ProcessoId")]
        public virtual Processo Processo { get; set; }
    }
}