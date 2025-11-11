using System.Diagnostics;
namespace EscritorioAdvocacia.Models { 

    public class Cliente{

    public int Id { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; } // Ou CNPJ
    public string Email { get; set; }
    public string Telefone { get; set; }

    // Um cliente pode ter vários processos
    public virtual ICollection<Processo> Processos { get; set; }
    }
}