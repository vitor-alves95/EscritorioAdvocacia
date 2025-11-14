using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscritorioAdvocacia.Models
{
    public class Processo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O 'Número Unificado' é obrigatório.")]
        [Display(Name = "Nº Unificado (CNJ)")]

        public string NumeroUnificado { get; set; }

        [Required(ErrorMessage = "O 'Título' é obrigatório.")]
        public string Titulo { get; set; } 
        public string StatusAndamento { get; set; } 
        public string DescricaoAndamento { get; set; } 
        public DateTime DataAbertura { get; set; }

           
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int AdvogadoId { get; set; }
        public virtual Advogado AdvogadoResponsavel { get; set; }
      
        public int TipoProcessoId { get; set; }
        public virtual TipoProcesso TipoProcesso { get; set; }

        public int VaraOrigemId { get; set; }
        public virtual VaraOrigem VaraOrigem { get; set; }
    }
}
