using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Boosters
{
    public class BoostersView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button freezeBuyButton;
        [SerializeField] private Button swapBuyButton;
        [SerializeField] private Button oneExtraBuyButton;
        [SerializeField] private Button comeLineBuyButton;
        [SerializeField] private TMP_Text freezeCountText;
        [SerializeField] private TMP_Text swapCountText;
        [SerializeField] private TMP_Text oneExtraCountText;
        [SerializeField] private TMP_Text comeLineCountText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        
        public event Action OnClose;
        public event Action OnFreezeBuyClick;
        public event Action OnSwapBuyClick;
        public event Action OnOneExtraBuyClick;
        public event Action OnComeLineBuyClick;

        public void ResizeToIpad()
        {
            verticalLayoutGroup.spacing = 50;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            Subscribe();
        }

        public void Hide()
        {
            Unsubscribe();
            gameObject.SetActive(false);
        }

        public void UpdateGold(int gold)
        {
            goldText.text = gold.ToString();
        }

        public void SetCounts(int freezeCount, int swapCount, int oneExtraCount, int comeLineCount)
        {
            freezeCountText.text = freezeCount.ToString();
            swapCountText.text = swapCount.ToString();
            oneExtraCountText.text = oneExtraCount.ToString();
            comeLineCountText.text = comeLineCount.ToString();
        }

        public void ActivateFreezeBuyButton(bool value)
        {
            freezeBuyButton.interactable = value;
        }

        public void ActivateSwapBuyButton(bool value)
        {
            swapBuyButton.interactable = value;
        }

        public void ActivateOneExtraBuyButton(bool value)
        {
            oneExtraBuyButton.interactable = value;
        }

        public void ActivateComeLineBuyButton(bool value)
        {
            comeLineBuyButton.interactable = value;
        }

        private void Subscribe()
        {
            closeButton.onClick.AddListener(Click);
            freezeBuyButton.onClick.AddListener(FreezeBuyClick);
            swapBuyButton.onClick.AddListener(SwapBuyClick);
            oneExtraBuyButton.onClick.AddListener(OneExtraBuyClick);
            comeLineBuyButton.onClick.AddListener(ComeLineBuyClick);
        }

        private void Unsubscribe()
        {
            closeButton.onClick.RemoveAllListeners();
            freezeBuyButton.onClick.RemoveAllListeners();
            swapBuyButton.onClick.RemoveAllListeners();
            oneExtraBuyButton.onClick.RemoveAllListeners();
            comeLineBuyButton.onClick.RemoveAllListeners();
        }

        private void Click()
        {
            closeButton.onClick.RemoveAllListeners();
            OnClose?.Invoke();
        }

        private void FreezeBuyClick()
        {
            OnFreezeBuyClick?.Invoke();
        }

        private void SwapBuyClick()
        {
            OnSwapBuyClick?.Invoke();
        }

        private void OneExtraBuyClick()
        {
            OnOneExtraBuyClick?.Invoke();
        }

        private void ComeLineBuyClick()
        {
            OnComeLineBuyClick?.Invoke();
        }
    }
}