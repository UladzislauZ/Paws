using System;
using UnityEngine;

namespace FruityPaw.Scripts.Fruits
{
    [Serializable]
    public class Fruit
    {
        public Sprite sprite;
        public FruitType fruitType;
        public int minPrice;
        public int maxPrice;
    }
}