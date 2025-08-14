using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Level
{
    public class StatisticsView : MonoBehaviour
    {
        [SerializeField] private Image[] images;

        private int _indexFruit;

        public void Initialize()
        {
            _indexFruit = 0;
            foreach (var image in images)
            {
                image.gameObject.SetActive(false);
            }
        }

        public void AddFruit(Sprite fruitSprite)
        {
            images[_indexFruit].sprite = fruitSprite;
            images[_indexFruit].gameObject.SetActive(true);
            _indexFruit++;
        }
    }
}