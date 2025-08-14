using System;
using System.Linq;
using UnityEngine;

namespace FruityPaw.Scripts.Backgrounds
{
    [CreateAssetMenu(fileName = "BackgroundConfig", menuName = "FruityPaw/BackgroundConfig")]
    public class BackgroundConfig : ScriptableObject
    {
        public BackgroundItem[] backgrounds;

        public Sprite GetBackgroundSprite(BackgroundType backgroundType, bool isIpad)
        {
            var item = backgrounds.First(x => x.backgroundType == backgroundType);
            return isIpad ? item.backgroundIpadSprite : item.backgroundSprite;
        }
    }

    [Serializable]
    public class BackgroundItem
    {
        public BackgroundType backgroundType;
        public Sprite backgroundSprite;
        public Sprite backgroundIpadSprite;
    }
}