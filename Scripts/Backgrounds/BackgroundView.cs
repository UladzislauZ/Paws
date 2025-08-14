using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Backgrounds
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class BackgroundView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        [SerializeField] private Vector2 sliceOffset;

        private bool _isPortraitOrientation;

        private const int IpadHeight = 1500;
        private const float IphonePortraitAspectRatio = 0.46f;
        private const float IpadPortraitAspectRatio = 0.75f;
        private const float IpadLandscapeAspectRatio = 1.2f;
        private const float IphoneLandscapeAspectRatio = 2.2f;
        private const float IphoneXRRatio = 1.87f;

        public bool IsIpadOrientation => GetCurrentDeviceHeight() >= IpadHeight;

        public void AdjustResolution(Sprite sprite)
        {
            backgroundImage.color = Color.white;
            SetBackgroundOrientation();
            SetImageResolution(sprite);
            SetSlice(IsIpadOrientation);
            rectTransform.sizeDelta = Vector2.zero;
        }

        private void SetBackgroundOrientation()
        {
            _isPortraitOrientation = Screen.height > Screen.width;
        }

        private int GetCurrentDeviceHeight()
        {
            return Screen.height > Screen.width == false ? Screen.height : Screen.width;
        }

        private void SetImageResolution(Sprite sprite)
        {
            backgroundImage.sprite = sprite;
        }

        private void SetSlice(bool isiPad)
        {
            ConfigureAspectRatioFitter(isiPad);
            ApplyOffset(isiPad);
        }

        private void ApplyOffset(bool isiPad)
        {
            rectTransform.anchoredPosition = isiPad || GetAspectRatio() >= IphoneXRRatio
                ? Vector2.zero
                : new Vector2(sliceOffset.x, sliceOffset.y);
        }

        private void ConfigureAspectRatioFitter(bool isiPad)
        {
            if (_isPortraitOrientation == false) SetAspectRatioByLandscape(isiPad);
            else SetAspectRatioByPortrait(isiPad);
        }

        private void SetAspectRatioByLandscape(bool isiPad)
        {
            aspectRatioFitter.aspectMode = isiPad
                ? AspectRatioFitter.AspectMode.WidthControlsHeight
                : AspectRatioFitter.AspectMode.HeightControlsWidth;
            aspectRatioFitter.aspectRatio = isiPad ? IpadLandscapeAspectRatio : IphoneLandscapeAspectRatio;
        }

        private void SetAspectRatioByPortrait(bool isiPad)
        {
            aspectRatioFitter.aspectMode = isiPad
                ? AspectRatioFitter.AspectMode.HeightControlsWidth
                : AspectRatioFitter.AspectMode.WidthControlsHeight;
            aspectRatioFitter.aspectRatio = isiPad ? IpadPortraitAspectRatio : IphonePortraitAspectRatio;
        }

        private float GetAspectRatio()
        {
            var screenSize = _isPortraitOrientation == false
                ? new Vector2Int(Screen.width, Screen.height)
                : new Vector2Int(Screen.height, Screen.width);
            return screenSize.x / screenSize.y;
        }
    }

}