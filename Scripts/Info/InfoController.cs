using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts.Info
{
    public class InfoController : IViewController
    {
        private readonly InfoView _infoView;
        private readonly BackgroundController _backgroundController;
        private readonly SoundController _soundController;

        private UniTaskCompletionSource<ViewType> _tcs;

        public InfoController(InfoView infoView,
            BackgroundController backgroundController,
            SoundController soundController)
        {
            _infoView = infoView;
            _backgroundController = backgroundController;
            _soundController = soundController;
        }
        
        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Game);
            _tcs = new UniTaskCompletionSource<ViewType>();
            _infoView.OnClose += Close;
            _infoView.Show();
            await _tcs.Task;
            _infoView.Hide();
            _infoView.OnClose -= Close;
            return _tcs.GetResult(0);
        }

        private void Close()
        {
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(ViewType.Main);
        }
    }
}