using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Main
{
    public class MainView : MonoBehaviour
    {
        [SerializeField] private Button soundButton;
        [SerializeField] private Image soundImage;
        [SerializeField] private Button infoButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button marketButton;
        [SerializeField] private Button boostersButton;
        [SerializeField] private Button dailyBonusButton;
        [SerializeField] private TMP_Text totalWinsText;
        [SerializeField] private RectTransform blockRectTransform;

        public event Action<ViewType> OnClick;
        public event Action OnSoundChange;

        public void ResizeToIpad()
        {
            blockRectTransform.anchoredPosition = new Vector2(0, 100f);
        }

        public void Show(string totalWins, bool isActiveDaily)
        {
            gameObject.SetActive(true);
            Subscribe();
            totalWinsText.text = totalWins;
            ActivateDailyBonus(isActiveDaily);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateSoundImage(Sprite sprite)
        {
            soundImage.sprite = sprite;
        }

        private void ActivateDailyBonus(bool isActive)
        {
            if (isActive)
            {
                dailyBonusButton.onClick.AddListener(DailyBonusClick);
                return;
            }
            
            dailyBonusButton.interactable = false;
        }

        private void Subscribe()
        {
            soundButton.onClick.AddListener(SoundClick);
            infoButton.onClick.AddListener(InfoClick);
            playButton.onClick.AddListener(PlayClick);
            marketButton.onClick.AddListener(MarketClick);
            boostersButton.onClick.AddListener(BoostersClick);
        }

        private void Unsubscribe()
        {
            soundButton.onClick.RemoveAllListeners();
            infoButton.onClick.RemoveAllListeners();
            playButton.onClick.RemoveAllListeners();
            marketButton.onClick.RemoveAllListeners();
            boostersButton.onClick.RemoveAllListeners();
            dailyBonusButton.onClick.RemoveAllListeners();
        }

        private void SoundClick()
        {
            OnSoundChange?.Invoke();
        }

        private void InfoClick()
        {
            Unsubscribe();
            OnClick?.Invoke(ViewType.Info);
        }

        private void PlayClick()
        {
            Unsubscribe();
            OnClick?.Invoke(ViewType.Play);
        }

        private void MarketClick()
        {
            Unsubscribe();
            OnClick?.Invoke(ViewType.Market);
        }

        private void BoostersClick()
        {
            Unsubscribe();
            OnClick?.Invoke(ViewType.Boosters);
        }

        private void DailyBonusClick()
        {
            Unsubscribe();
            OnClick?.Invoke(ViewType.DailyBonus);
        }
    }
}