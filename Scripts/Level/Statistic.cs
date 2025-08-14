using FruityPaw.Scripts.Fruits;

namespace FruityPaw.Scripts.Level
{
    public class Statistic
    {
        public FruitType[] Rewards = new FruitType[3];
        
        public int CountReward{get; private set;}

        public void Clear()
        {
            Rewards = new FruitType[3];
            CountReward = 0;
        }

        public void AddReward(FruitType fruit)
        {
            if (CountReward == 3) return;
            Rewards[CountReward] = fruit;
            CountReward++;
        }
    }
}