using System;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace FruityPaw.Scripts.Info
{
    public class InfoView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject ipadScrollView;
        [SerializeField] private GameObject iphoneScrollView;
        
        public event Action OnClose;

        public void ResizeToIpad()
        {
            ipadScrollView.SetActive(true);
            iphoneScrollView.SetActive(false);
        }

        public void Show()
        {
            closeButton.onClick.AddListener(Click);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            closeButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

        private void Click()
        {
            closeButton.onClick.RemoveAllListeners();
            OnClose?.Invoke();
        }
    }
}