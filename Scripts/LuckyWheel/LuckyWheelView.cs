using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.LuckyWheel
{
    public class LuckyWheelView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button spinButton;
        [SerializeField] private RectTransform spinButtonRectTransform;
        [SerializeField] private TMP_Text spinButtonText;
        [SerializeField] private RectTransform mainLogoRectTransform;
        [SerializeField] private RectTransform wheelRectTransform;
        [SerializeField] private RectTransform spinRectTransform;
        [SerializeField] private Image boosterImage;
        [SerializeField] private TMP_Text wonBoosterText;
        
        public event Action OnClose;
        public event Action OnSpin;

        public void ResizeToIpad()
        {
            spinButtonRectTransform.anchoredPosition = new Vector2(0, -30f);
            spinRectTransform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        }

        public void Show()
        {
            spinButtonText.text = "Spin";
            closeButton.gameObject.SetActive(true);
            mainLogoRectTransform.gameObject.SetActive(true);
            spinRectTransform.gameObject.SetActive(true);
            spinButton.transform.localScale = Vector3.one;
            closeButton.onClick.AddListener(CloseClick);
            spinButton.onClick.AddListener(SpinClick);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            closeButton.transform.localScale = Vector3.one;
            wheelRectTransform.transform.rotation = Quaternion.Euler(Vector3.zero);
            wonBoosterText.transform.localScale = Vector3.zero;
            boosterImage.transform.localScale = Vector3.zero;
        }

        public async UniTask HideSpinButtonAsync()
        {
            closeButton.transform.DOScale(Vector3.zero, 0.5f);
            spinButton.transform.DOScale(Vector3.zero, 0.5f);
            await UniTask.Delay(500);
        }

        public async UniTask SpinWheelAsync(Vector3 value, float duration)
        {
            wheelRectTransform.transform.DORotate(value, duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            var delayTime = duration * 1000f;
            await UniTask.Delay((int)delayTime);
        }

        public async UniTask ShowWonBooster(Sprite boosterSprite)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.gameObject.SetActive(false);
            mainLogoRectTransform.gameObject.SetActive(false);
            spinRectTransform.gameObject.SetActive(false);
            wonBoosterText.transform.DOScale(Vector3.one, 0.5f);
            await UniTask.Delay(500);
            boosterImage.sprite = boosterSprite;
            boosterImage.transform.DOScale(new Vector3(2f, 2f, 2f), 0.5f);
            await UniTask.Delay(500);
            spinButtonText.text = "Get";
            spinButton.transform.DOScale(Vector3.one, 0.5f);
            await UniTask.Delay(500);
            spinButton.onClick.AddListener(CloseClick);
        }

        private void CloseClick()
        {
            closeButton.onClick.RemoveAllListeners();
            spinButton.onClick.RemoveAllListeners();
            OnClose?.Invoke();
        }

        private void SpinClick()
        {
            spinButton.onClick.RemoveAllListeners();
            OnSpin?.Invoke();
        }
    }
}