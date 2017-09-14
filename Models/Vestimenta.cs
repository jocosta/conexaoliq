namespace CTX.Bot.ConexaoLiq.Models
{
    public class Vestimenta
    {

        public Vestimenta(string chave, string texto, string imagem)
        {
            Chave = chave;
            Texto = texto;
            Imagem = new Imagem { Caminho = imagem };
        }
        public string Chave { get; set; }

        public string Texto { get; set; }

        public Imagem Imagem { get; set; }

    }
}