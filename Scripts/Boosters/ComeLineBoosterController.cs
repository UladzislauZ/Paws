using System;
using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;

namespace FruityPaw.Scripts.Boosters
{
    public class ComeLineBoosterController : IBoosterController
    {
        private readonly DataController _dataController;

        public event Action OnComeLineActivate;

        public ComeLineBoosterController(DataController dataController)
        {
            _dataController = dataController;
        }

        public async UniTask Start()
        {
            _dataController.SpendBooster(BoosterType.ComeLine);
            OnComeLineActivate?.Invoke();
            await UniTask.Yield();
        }
    }
}