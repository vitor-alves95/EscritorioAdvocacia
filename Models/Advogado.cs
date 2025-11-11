using System.Diagnostics;
namespace EscritorioAdvocacia.Models { 

    public class Advogado { 

    public int Id { get; set; }
    public string Nome { get; set; }
    public string OAB { get; set; }
    public string Email { get; set; }

    // Um advogado pode ser responsável por vários processos
    public virtual ICollection<Processo> Processos { get; set; }
    }
}