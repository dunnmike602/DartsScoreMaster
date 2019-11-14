using System.Runtime.Serialization;
using DartsScoreMaster.Services.Interfaces;

namespace DartsScoreMaster.ViewModels
{
    [DataContract]
    public abstract class VoiceControlViewModel : BaseViewModel
    {
        protected ICommentaryPlayer CommentaryPlayer { get; }
        public IDialogService DialogService { get; }
        public bool IsSoundSupportEnabled { get; set; }

        protected VoiceControlViewModel(ICommentaryPlayer commentaryPlayer, IDialogService dialogService)
        {
            CommentaryPlayer = commentaryPlayer;
            DialogService = dialogService;
        }
    }
}