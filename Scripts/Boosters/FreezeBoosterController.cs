using System.Linq;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts.Boosters
{
    public class FreezeBoosterController : IBoosterController
    {
        private readonly LevelModel _levelModel;
        private readonly DataController _dataController;
        private readonly SoundController _soundController;

        private UniTaskCompletionSource _tcs;

        public FreezeBoosterController(LevelModel levelModel,
            DataController dataController,
            SoundController soundController)
        {
            _levelModel = levelModel;
            _dataController = dataController;
            _soundController = soundController;
        }

        public async UniTask Start()
        {
            _dataController.SpendBooster(BoosterType.Freeze);
            _tcs = new UniTaskCompletionSource();
            var botPaws = _levelModel.BotPaws.Where(x => x.RowIndexField != 7).ToArray();
            foreach (var botPaw in botPaws)
            {
                botPaw.ActivateColliderPaw(true);
                botPaw.OnPawSelected += SelectPaw;
            }
            await _tcs.Task;
            foreach (var botPaw in botPaws)
            {
                botPaw.ActivateColliderPaw(false);
                botPaw.OnPawSelected -= SelectPaw;
            }
        }

        private void SelectPaw(Paw paw)
        {
            _soundController.PlaySound(SoundType.PawSelect);
            paw.Freeze();
            _tcs.TrySetResult();
        }
    }
}