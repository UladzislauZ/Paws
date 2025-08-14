using Cysharp.Threading.Tasks;

namespace FruityPaw.Scripts
{
    public interface IViewController
    {
        UniTask<ViewType> Start();
    }
}