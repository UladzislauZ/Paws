using System;
using System.Linq;
using UnityEngine;

namespace FruityPaw.Scripts.Boosters
{
    [CreateAssetMenu(fileName = "BoostersConfig", menuName = "FruityPaw/BoostersConfig")]
    public class BoostersConfig : ScriptableObject
    {
        public BoostersItem[] boosters;

        public Sprite GetBoosterSprite(BoosterType boosterType)
        {
            return boosters.First(x=>x.type == boosterType).sprite;
        }
    }
    
    [Serializable]
    public class BoostersItem
    {
        public BoosterType type;
        public Sprite sprite;
    }
}