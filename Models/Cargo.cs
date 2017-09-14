namespace CTX.Bot.ConexaoLiq.Models
{
    public class Cargo
    {
        public string DescricaoCargo { get; set; }

        public string NomePessoa { get; set; }

        public string[] Tags { get; set; }

        public Imagem Imagem { get; set; }
    }
}