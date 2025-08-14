using System;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Level.FieldObjects
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameObject redStroke;
        [SerializeField] private GameObject goldStroke;
        [SerializeField] private Button button;
        [SerializeField] private Image squareImage;
        [SerializeField] private Animator animatorSquare;
        
        public event Action<int, int> OnClicked; 
        
        public RectTransform RectTransform => rectTransform;
        public int FieldRowIndex { get; private set; }
        public int FieldColumnIndex { get; private set; }

        public void SetIndex(int rowIndex, int columnIndex)
        {
            FieldRowIndex = rowIndex;
            FieldColumnIndex = columnIndex;
        }

        public void SelectField(bool value)
        {
            redStroke.gameObject.SetActive(value);
            if (goldStroke != null)
                goldStroke.gameObject.SetActive(!value);
        }

        public void ActivateClickable()
        {
            button.interactable = true;
            button.onClick.AddListener(Click);
            animatorSquare.enabled = true;
        }

        public void DeactivateClickable()
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
            animatorSquare.enabled = false;
            var color = squareImage.color;
            color.a = 0;
            squareImage.color = color;
        }

        private void Click()
        {
            button.onClick.RemoveAllListeners();
            OnClicked?.Invoke(FieldRowIndex, FieldColumnIndex);
        }
    }
}