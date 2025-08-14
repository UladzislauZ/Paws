using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Popup
{
    public class EndView : MonoBehaviour
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private GameObject winLabel;
        [SerializeField] private GameObject loseLabel;
        [SerializeField] private TMP_Text collectText;
        [SerializeField] private FruitRewardView[] fruitRewardViews;
        
        private UniTaskCompletionSource<ViewType> _tcs;

        public async UniTask<ViewType> Show(bool isWin, int countReward, Sprite[] rewardSprites)
        {
            _tcs = new UniTaskCompletionSource<ViewType>();
            if (isWin)
                winLabel.SetActive(true);
            else 
                loseLabel.SetActive(true);

            Subscribe();
            collectText.text = $"You have collected\n{countReward} fruits";
            gameObject.SetActive(true);
            if (countReward != 0)
                for (var i = 0; i < countReward; i++)
                    fruitRewardViews[i].Show(rewardSprites[i]);
            
            await _tcs.Task;
            
            foreach (var rewardView in fruitRewardViews)
            {
                rewardView.Hide();
            }
            
            Unsubscribe();
            winLabel.SetActive(false);
            loseLabel.SetActive(false);
            gameObject.SetActive(false);
            return _tcs.GetResult(0);
        }

        private void Subscribe()
        {
            nextButton.onClick.AddListener(NextClick);
            homeButton.onClick.AddListener(HomeClick);
        }

        private void Unsubscribe()
        {
            nextButton.onClick.RemoveAllListeners();
            homeButton.onClick.RemoveAllListeners();
        }

        private void NextClick()
        {
            Unsubscribe();
            _tcs.TrySetResult(ViewType.Play);
        }

        private void HomeClick()
        {
            Unsubscribe();
            _tcs.TrySetResult(ViewType.Main);
        }
    }
}