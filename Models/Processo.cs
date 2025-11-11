namespace EscritorioAdvocacia.Models
{
    public class Processo
    {
        public int Id { get; set; }
        public string NumeroUnificado { get; set; } // O número do processo (CNJ)
        public string Titulo { get; set; } // Um título amigável (ex: "Silva vs. Empresa X")
        public string StatusAndamento { get; set; } // "Em andamento", "Concluído", "Arquivado"
        public string DescricaoAndamento { get; set; } // Última atualização (o que o cliente verá)
        public DateTime DataAbertura { get; set; }

        // --- Chaves Estrangeiras (Relacionamentos) ---

        // Relacionamento com Cliente (Quem é o cliente deste processo?)
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        // Relacionamento com Advogado (Quem é o responsável?)
        public int AdvogadoId { get; set; }
        public virtual Advogado AdvogadoResponsavel { get; set; }

        // Relacionamento com Tipo
        public int TipoProcessoId { get; set; }
        public virtual TipoProcesso TipoProcesso { get; set; }

        // Relacionamento com Vara
        public int VaraOrigemId { get; set; }
        public virtual VaraOrigem VaraOrigem { get; set; }
    }
}
