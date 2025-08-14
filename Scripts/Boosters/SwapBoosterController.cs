using System;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Sounds;

namespace FruityPaw.Scripts.Boosters
{
    public class SwapBoosterController : IBoosterController
    {
        private readonly DataController _dataController;
        private readonly LevelModel _levelModel;
        private readonly SoundController _soundController;

        private Paw _selectedPaw;

        public bool IsSwapActivate { get; private set; }

        public event Action OnStartBooster;
        public event Action OnEndBooster;

        public SwapBoosterController(DataController dataController,
            LevelModel levelModel,
            SoundController soundController)
        {
            _dataController = dataController;
            _levelModel = levelModel;
            _soundController = soundController;
        }

        public async UniTask Start()
        {
            OnStartBooster?.Invoke();
            _dataController.SpendBooster(BoosterType.Swap);
            IsSwapActivate = true;
            foreach (var playerPaw in _levelModel.PlayerPaws)
            {
                playerPaw.ActivateColliderPaw(true);
                playerPaw.OnPawSelected += PawSelect;
            }

            await UniTask.Yield();
        }

        private void PawSelect(Paw paw)
        {
            _soundController.PlaySound(SoundType.PawSelect);
            if (_selectedPaw == null)
            {
                _selectedPaw = paw;
                _levelModel.GameFields[_selectedPaw.RowIndexField, _selectedPaw.ColumnIndexField]
                    .SelectField(true);
                return;
            }
            
            if(_selectedPaw == paw) return;

            
            foreach (var playerPaw in _levelModel.PlayerPaws)
            {
                playerPaw.ActivateColliderPaw(false);
                playerPaw.OnPawSelected -= PawSelect;
            }
            
            _levelModel.GameFields[_selectedPaw.RowIndexField, _selectedPaw.ColumnIndexField]
                .SelectField(false);
            SwapPaws(_selectedPaw, paw).Forget();
        }

        private async UniTask SwapPaws(Paw firstPaw, Paw secondPaw)
        {
            var firstRowIndex = firstPaw.RowIndexField;
            var firstColumnIndex = firstPaw.ColumnIndexField;
            var secondRowIndex = secondPaw.RowIndexField;
            var secondColumnIndex = secondPaw.ColumnIndexField;
            await UniTask.WhenAll(firstPaw.MovePaw(
                    _levelModel.GameFields[secondRowIndex, secondColumnIndex].RectTransform.position, secondRowIndex,
                    secondColumnIndex),
                secondPaw.MovePaw(
                    _levelModel.GameFields[firstRowIndex, firstColumnIndex].RectTransform.position, firstRowIndex,
                    firstColumnIndex));
            IsSwapActivate = false;
            _selectedPaw = null;
            OnEndBooster?.Invoke();
        }
    }
}