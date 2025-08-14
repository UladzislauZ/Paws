using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Level.FieldObjects;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Popup;
using Object = UnityEngine.Object;

namespace FruityPaw.Scripts.Level
{
    public class LevelController
    {
        private readonly LevelView _levelView;
        private readonly World _world;
        private readonly LevelModel _levelModel;
        private readonly PopupController _popupController;
        private readonly FruitsConfig _fruitsConfig;
        private readonly PlayerController _playerController;
        private readonly BotController _botController;
        private readonly PawConfig _pawConfig;
        private readonly DataController _dataController;

        private CancellationTokenSource _cts;

        public event Action<ViewType> OnClose;

        public LevelController(LevelView levelView,
            World world,
            LevelModel levelModel,
            PopupController popupController,
            FruitsConfig fruitsConfig,
            PlayerController playerController,
            BotController botController,
            PawConfig pawConfig,
            DataController dataController)
        {
            _levelView = levelView;
            _world = world;
            _levelModel = levelModel;
            _popupController = popupController;
            _fruitsConfig = fruitsConfig;
            _playerController = playerController;
            _botController = botController;
            _pawConfig = pawConfig;
            _dataController = dataController;
        }

        public void Start()
        {
            _levelModel.BotPaws = new Paw[6];
            _levelModel.PlayerPaws = new Paw[6];
            _levelModel.OnEnd = false;
            CreateBotPaws();
            CreatePlayerPaws();
            CreateGameFields();
        }

        public void CreateLevel()
        {
            _cts = new CancellationTokenSource();
            for (var i = 0; i < _levelModel.PlayerPaws.Length; i++)
            {
                _levelModel.PlayerPaws[i].UpdatePosition(_levelView.PlayerPawsFields[i].RectTransform.position,
                    _levelView.PlayerPawsFields[i].FieldRowIndex,_levelView.PlayerPawsFields[i].FieldColumnIndex);
                _levelModel.PlayerPaws[i].gameObject.SetActive(true);
                _levelModel.BotPaws[i].UpdatePosition(_levelView.BotPawsFields[i].RectTransform.position,
                    _levelView.BotPawsFields[i].FieldRowIndex,_levelView.BotPawsFields[i].FieldColumnIndex);
                _levelModel.BotPaws[i].gameObject.SetActive(true);
            }
            
            StartLevelSteps().Forget();
        }

        public void StopLevel()
        {
            _cts.Cancel();
            _levelModel.Clear();
            for (var i = 0; i < _levelModel.PlayerPaws.Length; i++)
            {
                _levelModel.PlayerPaws[i].gameObject.SetActive(false);
                _levelModel.BotPaws[i].gameObject.SetActive(false);
            }
        }

        private void CreateBotPaws()
        {
            _levelModel.BotPaws[0] = CreatePaw(true, TypePaw.Small);
            _levelModel.BotPaws[1] = CreatePaw(true, TypePaw.Small);
            _levelModel.BotPaws[2] = CreatePaw(true, TypePaw.Normal);
            _levelModel.BotPaws[3] = CreatePaw(true, TypePaw.Normal);
            _levelModel.BotPaws[4] = CreatePaw(true, TypePaw.Big);
            _levelModel.BotPaws[5] = CreatePaw(true, TypePaw.Big);
            foreach (var botPaw in _levelModel.BotPaws)
            {
                botPaw.gameObject.SetActive(false);
            }
        }

        private void CreatePlayerPaws()
        {
            _levelModel.PlayerPaws[0] = CreatePaw(false, TypePaw.Small);
            _levelModel.PlayerPaws[1] = CreatePaw(false, TypePaw.Small);
            _levelModel.PlayerPaws[2] = CreatePaw(false, TypePaw.Normal);
            _levelModel.PlayerPaws[3] = CreatePaw(false, TypePaw.Normal);
            _levelModel.PlayerPaws[4] = CreatePaw(false, TypePaw.Big);
            _levelModel.PlayerPaws[5] = CreatePaw(false, TypePaw.Big);
            foreach (var playerPaw in _levelModel.PlayerPaws)
            {
                playerPaw.gameObject.SetActive(false);
            }
        }

        private Paw CreatePaw(bool isBot, TypePaw typePaw)
        {
            var paw = Object.Instantiate(_pawConfig.pawPrefab, _world.transform);
            var sprite = _pawConfig.GetPawSprite(isBot, typePaw);
            paw.Initialize(typePaw, sprite, isBot);
            return paw;
        }

        private void CreateGameFields()
        {
            _levelModel.GameFields = new GameField[8, 6];
            for (var i = 0; i < _levelView.BotPawsFields.Length; i++)
            {
                _levelModel.GameFields[0, i] = _levelView.BotPawsFields[i];
                _levelModel.GameFields[0, i].SetIndex(0, i);
            }
            for (var i = 0; i < _levelView.GameFields.Length; i++)
            {
                var rowIndex = i / 6 + 1;
                var columnIndex = i % 6;
                _levelModel.GameFields[rowIndex, columnIndex] = _levelView.GameFields[i];
                _levelModel.GameFields[rowIndex, columnIndex].SetIndex(rowIndex, columnIndex);
            }
            for (var i = 0; i < _levelView.PlayerPawsFields.Length; i++)
            {
                _levelModel.GameFields[7, i] = _levelView.PlayerPawsFields[i];
                _levelModel.GameFields[7, i].SetIndex(7, i);
            }
        }

        private async UniTask StartLevelSteps()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Round();
                await TryEndLevel();
            }
        }

        private async UniTask Round()
        {
            await _playerController.PlayerStepAsync();
            if (_levelModel.OnEnd) return;
            if (_levelModel.IsExtraMove)
            {
                _levelModel.IsExtraMove = false;
                await _playerController.PlayerStepAsync(true);
            }
            
            if (_levelModel.OnEnd) return;
            
            await _botController.BotStepAsync();
        }

        private async UniTask TryEndLevel()
        {
            if (!_levelModel.OnEnd) return;

            var isWin = _levelModel.PlayerStatistic.CountReward == 3;
            var fruitSprites = _levelModel.PlayerStatistic.CountReward == 0
                ? null
                : _fruitsConfig.GetFruitSprites(_levelModel.PlayerStatistic.Rewards);
            var result = await _popupController.ShowEndPopup(
                isWin, 
                _levelModel.PlayerStatistic.CountReward,
                fruitSprites);
            if (isWin) _dataController.IncreasePlayerWin();
            else _dataController.IncreaseBotWin();
            
            OnClose?.Invoke(result);
            _cts.Cancel();
        }
    }
}