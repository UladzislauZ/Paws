using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Popup
{
    public class QuestionView : MonoBehaviour
    {
        [SerializeField] private Button firstButton;
        [SerializeField] private Button secondButton;
        [SerializeField] private TMP_Text firstButtonText;
        [SerializeField] private TMP_Text secondButtonText;
        [SerializeField] private TMP_Text questionText;
        
        public event Action<bool> OnButtonPressed; 

        public void Show(
            string question,
            string firstTextButton,
            string secondTextButton)
        {
            questionText.text = question;
            firstButtonText.text = firstTextButton;
            secondButtonText.text = secondTextButton;
            gameObject.SetActive(true);
            firstButton.onClick.AddListener(FirstClick);
            secondButton.onClick.AddListener(SecondClick);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void FirstClick()
        {
            firstButton.onClick.RemoveAllListeners();
            secondButton.onClick.RemoveAllListeners();
            OnButtonPressed?.Invoke(true);
        }

        private void SecondClick()
        {
            firstButton.onClick.RemoveAllListeners();
            secondButton.onClick.RemoveAllListeners();
            OnButtonPressed?.Invoke(false);
        }
    }
}