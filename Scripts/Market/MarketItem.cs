using System;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Fruits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Market
{
    public class MarketItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private TMP_Text cellCountText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button cellButton;
        
        private int _maxCellCount;
        private int _currentCellCount;
        
        public FruitType FruitType { get; private set; }
        
        public event Action<FruitType, int> OnBuyClick;

        public void Initialize(FruitType fruitType,
            Sprite sprite)
        {
            FruitType = fruitType;
            image.sprite = sprite;
            leftButton.onClick.AddListener(LeftClick);
            rightButton.onClick.AddListener(RightClick);
            cellButton.onClick.AddListener(CellClick);
        }

        public void UpdateCount(int count, int price)
        {
            countText.text = count.ToString();
            _maxCellCount = count;
            _currentCellCount = 1;
            cellButton.interactable = count != 0;
            cellCountText.text = _currentCellCount.ToString();
            priceText.text = price.ToString();
        }

        private void LeftClick()
        {
            if (_maxCellCount == 0) return;
            
            if (_currentCellCount == 1)
                _currentCellCount = _maxCellCount;
            else
                _currentCellCount--;
            
            cellCountText.text = _currentCellCount.ToString();
        }

        private void RightClick()
        {
            if (_maxCellCount == 0) return;

            if (_currentCellCount == _maxCellCount)
                _currentCellCount = 1;
            else
                _currentCellCount++;
            
            cellCountText.text = _currentCellCount.ToString();
        }

        private void CellClick()
        {
            OnBuyClick?.Invoke(FruitType, _currentCellCount);
        }
    }
}