using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Fruits;
using FruityPaw.Scripts.Level.Paws;
using FruityPaw.Scripts.Sounds;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace FruityPaw.Scripts
{
    public class Configs : MonoBehaviour
    {
        [SerializeField] private SoundConfig soundConfig;
        [SerializeField] private FruitsConfig fruitsConfig;
        [SerializeField] private BackgroundConfig backgroundConfig;
        [SerializeField] private BoostersConfig boostersConfig;
        [SerializeField] private PawConfig pawConfig;
        
        public void Configure(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(soundConfig);
            containerBuilder.RegisterInstance(fruitsConfig);
            containerBuilder.RegisterInstance(backgroundConfig);
            containerBuilder.RegisterInstance(boostersConfig);
            containerBuilder.RegisterInstance(pawConfig);
        }
    }
}