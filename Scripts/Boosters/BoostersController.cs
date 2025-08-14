using System;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Level;

namespace FruityPaw.Scripts.Boosters
{
    public class BoostersController
    {
        public const int FreezePrice = 15;
        public const int SwapPrice = 25;
        public const int OneExtraPrice = 30;
        public const int ComeLinePrice = 50;
        
        private readonly DataController _dataController;
        private readonly LevelView _levelView;
        private readonly FreezeBoosterController _freezeBoosterController;
        private readonly SwapBoosterController _swapBoosterController;
        private readonly ExtraMoveBoosterController _extraMoveBoosterController;
        private readonly ComeLineBoosterController _comeLineBoosterController;

        private bool _isLockedSystem;

        public BoostersController(DataController dataController,
            LevelView levelView,
            FreezeBoosterController freezeBoosterController,
            SwapBoosterController swapBoosterController,
            ExtraMoveBoosterController extraMoveBoosterController,
            ComeLineBoosterController comeLineBoosterController)
        {
            _dataController = dataController;
            _levelView = levelView;
            _freezeBoosterController = freezeBoosterController;
            _swapBoosterController = swapBoosterController;
            _extraMoveBoosterController = extraMoveBoosterController;
            _comeLineBoosterController = comeLineBoosterController;
        }

        public void Start()
        {
            _levelView.OnBoosterClick += BoosterClicked;
            UpdateClickableBoosters(false);
        }

        public void UpdateClickableBoosters(bool value)
        {
            _isLockedSystem = !value;
            foreach (BoosterType boosterType in Enum.GetValues(typeof(BoosterType)))
            {
                var clickable = value && _dataController.GetBoosterCount(boosterType) != 0;
                _levelView.UpdateBoosterClickable(boosterType, clickable);
            }
        }

        public bool TryBuyBooster(BoosterType boosterType)
        {
            return boosterType switch
            {
                BoosterType.Freeze => TryBuyFreeze(),
                BoosterType.Swap => TryBuySwap(),
                BoosterType.OneMove => TryBuyOneExtra(),
                BoosterType.ComeLine => TryBuyComeLine(),
                _ => throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null)
            };
        }

        private bool TryBuyFreeze()
        {
            if (_dataController.Gold < FreezePrice) return false;
            
            _dataController.SpendGold(FreezePrice);
            _dataController.AddFreeze();
            return true;
        }

        private bool TryBuySwap()
        {
            if (_dataController.Gold < SwapPrice) return false;
            
            _dataController.SpendGold(SwapPrice);
            _dataController.AddSwap();
            return true;
        }

        private bool TryBuyOneExtra()
        {
            if (_dataController.Gold < OneExtraPrice) return false;
            
            _dataController.SpendGold(OneExtraPrice);
            _dataController.AddOneExtra();
            return true;
        }

        private bool TryBuyComeLine()
        {
            if (_dataController.Gold < ComeLinePrice) return false;
            
            _dataController.SpendGold(ComeLinePrice);
            _dataController.AddComeLine();
            return true;
        }

        private void BoosterClicked(BoosterType boosterType)
        {
            if(_isLockedSystem) return;

            UpdateClickableBoosters(false);
            _isLockedSystem = true;
            switch (boosterType)
            {
                case BoosterType.Freeze:
                    _freezeBoosterController.Start().Forget();
                    break;
                case BoosterType.Swap:
                    _swapBoosterController.Start().Forget();
                    break;
                case BoosterType.OneMove:
                    _extraMoveBoosterController.Start().Forget();
                    break;
                case BoosterType.ComeLine:
                    _comeLineBoosterController.Start().Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
            }
        }
    }
}