using System;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FruityPaw.Scripts.LuckyWheel
{
    public class LuckyViewController : IViewController
    {
        private readonly LuckyWheelView _luckyWheelView;
        private readonly BackgroundController _backgroundController;
        private readonly BoostersConfig _boostersConfig;
        private readonly DataController _dataController;
        private readonly SoundController _soundController;

        private UniTaskCompletionSource<ViewType> _tcs;
        
        public LuckyViewController(LuckyWheelView luckyWheelView,
            BackgroundController backgroundController,
            BoostersConfig boostersConfig,
            DataController dataController,
            SoundController soundController)
        {
            _luckyWheelView = luckyWheelView;
            _backgroundController = backgroundController;
            _boostersConfig = boostersConfig;
            _dataController = dataController;
            _soundController = soundController;
        }
        
        public async UniTask<ViewType> Start()
        {
            _backgroundController.UpdateBackground(BackgroundType.Game);
            _tcs = new UniTaskCompletionSource<ViewType>();
            _luckyWheelView.OnClose += BackClick;
            _luckyWheelView.OnSpin += Spin;
            _luckyWheelView.Show();
            await _tcs.Task;
            _luckyWheelView.Hide();
            return _tcs.GetResult(0);
        }

        private void BackClick()
        {
            _luckyWheelView.OnClose -= BackClick;
            _luckyWheelView.OnSpin -= Spin;
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(ViewType.Main);
        }

        private void Spin()
        {
            _luckyWheelView.OnSpin -= Spin;
            _soundController.PlaySound(SoundType.Click);
            SpinAsync().Forget();
        }

        private async UniTask SpinAsync()
        {
            var boosterReward = Random.Range(0, 4);
            var boosterType = boosterReward switch
            {
                0 => BoosterType.Freeze,
                1 => BoosterType.Swap,
                2 => BoosterType.OneMove,
                _ => BoosterType.ComeLine
            };
            switch (boosterType)
            {
                case BoosterType.Swap:
                    _dataController.AddSwap();
                    break;
                case BoosterType.OneMove:
                    _dataController.AddOneExtra();
                    break;
                case BoosterType.ComeLine:
                    _dataController.AddComeLine();
                    break;
                case BoosterType.Freeze:
                    _dataController.AddFreeze();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _dataController.CollectDaily();
            await _luckyWheelView.HideSpinButtonAsync();
            _soundController.PlaySound(SoundType.Wheel);
            await _luckyWheelView.SpinWheelAsync(GetRotation(boosterType), 3f);
            _soundController.StopSound();
            await UniTask.Delay(1000);
            _backgroundController.UpdateBackground(BackgroundType.LuckyWheel);
            var boosterSprite = _boostersConfig.GetBoosterSprite(boosterType);
            await _luckyWheelView.ShowWonBooster(boosterSprite);
        }

        private Vector3 GetRotation(BoosterType boosterType)
        {
            var offset = boosterType switch
            {
                BoosterType.Freeze => Random.value > 0.5f ? 45 : 225,
                BoosterType.Swap => Random.value > 0.5f ? 180 : 315,
                BoosterType.OneMove => Random.value > 0.5f ? 0 : 135,
                BoosterType.ComeLine => Random.value > 0.5f ? 90 : 270,
                _ => 0
            };
            var z = 360 * 4 + offset;
            return new Vector3(0, 0, z);
        }
    }
}