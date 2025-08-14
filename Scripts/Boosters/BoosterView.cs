using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Boosters
{
    public class BoosterView : MonoBehaviour
    {
        [SerializeField] private BoosterType boosterType;
        [SerializeField] private Button boosterButton;
        [SerializeField] private TMP_Text boosterCountText;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
        
        public BoosterType BoosterType => boosterType;

        public event Action<BoosterType> OnBoosterClick;

        public void Activate()
        {
            boosterButton.onClick.AddListener(Click);
        }

        public void Deactivate()
        {
            boosterButton.onClick.RemoveAllListeners();
        }

        public void SetInteractable(bool interactable)
        {
            boosterButton.interactable = interactable;
        }

        public void UpdateCount(int boosterCount)
        {
            boosterCountText.text = boosterCount.ToString();
        }

        private void Click()
        {
            OnBoosterClick?.Invoke(boosterType);
        }
    }
}