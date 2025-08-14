using System.Linq;
using UnityEngine;

namespace FruityPaw.Scripts.Level.Paws
{
    [CreateAssetMenu(menuName = "FruityPaw/Paw Config", fileName = "Paw Config")]
    public class PawConfig : ScriptableObject
    {
        public PawItem[] paws;
        public Paw pawPrefab;

        public Sprite GetPawSprite(bool isBot, TypePaw typePaw)
        {
            return paws.First(x => x.typePaw == typePaw && x.isBotPaw == isBot).sprite;
        }
    }

    [System.Serializable]
    public class PawItem
    {
        public TypePaw typePaw;
        public bool isBotPaw;
        public Sprite sprite;
    }
}