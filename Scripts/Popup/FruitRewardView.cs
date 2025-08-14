using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Popup
{
    public class FruitRewardView : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;

        public void Show(Sprite fruitSprite)
        {
            rewardImage.sprite = fruitSprite;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}