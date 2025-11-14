using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscritorioAdvocacia.Models { 

    public class TipoProcesso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O 'Nome' do tipo é obrigatório.")]
        public string Nome { get; set; }
    }
}