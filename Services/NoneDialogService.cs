using System.Threading.Tasks;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using CTX.Bot.ConexaoLiq.Repositories;

namespace CTX.Bot.ConexaoLiq.Services
{
    public class NoneDialogService : DialogServiceBase, IDialogService
    {
        private readonly NoneRepository _noneRepository;
        public NoneDialogService(Activity activity, LuisResult result, bool textToSpeech) : base(activity, result, textToSpeech)
        {
            _noneRepository = new NoneRepository();
        }

        public async override Task Processar()
        {
            var nonemsg = _noneRepository.GetOne();
            if (!string.IsNullOrEmpty(nonemsg.Imagem.Caminho))
            {
                _activity.Attachments.Add(new Attachment()
                {
                    ContentUrl = $"{UrlApi}{nonemsg.Imagem.Caminho}",
                    ContentType = "image/gif",
                    Name = "Liq"
                });
            }
            await PostAsync(nonemsg.Texto);
        }
    }
}