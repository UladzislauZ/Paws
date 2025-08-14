using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Level;

namespace FruityPaw.Scripts.Boosters
{
    public class ExtraMoveBoosterController : IBoosterController
    {
        private readonly LevelModel _levelModel;
        private readonly DataController _dataController;

        public ExtraMoveBoosterController(LevelModel levelModel,
            DataController dataController)
        {
            _levelModel = levelModel;
            _dataController = dataController;
        }

        public async UniTask Start()
        {
            _levelModel.IsExtraMove = true;
            _dataController.SpendBooster(BoosterType.OneMove);
            await UniTask.Yield();
        }
    }
}