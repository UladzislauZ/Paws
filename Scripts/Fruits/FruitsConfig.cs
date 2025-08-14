using System.Linq;
using UnityEngine;

namespace FruityPaw.Scripts.Fruits
{
    [CreateAssetMenu(menuName = "FruityPaw/Fruits", fileName = "Fruits config")]
    public class FruitsConfig : ScriptableObject
    {
        public Fruit[] fruits;

        public Sprite[] GetFruitSprites(FruitType[] fruitTypes)
        {
            return fruitTypes.Select(fruitType => fruits.First(x => x.fruitType == fruitType).sprite).ToArray();
        }
    }
}