using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Market
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private RectMask2D rectMask;
        
        public void UpdateProgress(int progress, int maxProgress)
        {
            var index = maxProgress - progress;
            float value = index switch
            {
                2 => 20f,
                3 => 40f,
                4 => 60f,
                5 => 80f,
                _ => 0
            };

            var padding = rectMask.padding;
            padding.w = value;
            rectMask.padding = padding;
            progressText.text = progress.ToString();
        }
    }
}