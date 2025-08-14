using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Info;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.LuckyWheel;
using FruityPaw.Scripts.Main;
using FruityPaw.Scripts.Market;
using FruityPaw.Scripts.Popup;
using FruityPaw.Scripts.Sounds;
using UnityEngine;
using VContainer;

namespace FruityPaw.Scripts
{
    public class SceneViews : MonoBehaviour
    {
        [SerializeField] private SoundView soundView;
        [SerializeField] private BackgroundView backgroundView;
        [SerializeField] private MainView mainView;
        [SerializeField] private MarketView marketView;
        [SerializeField] private LuckyWheelView luckyWheelView;
        [SerializeField] private LevelView levelView;
        [SerializeField] private BoostersView boostersView;
        [SerializeField] private InfoView infoView;
        [SerializeField] private QuestionView questionView;
        [SerializeField] private World world;
        [SerializeField] private EndView endView;

        public void Configure(IContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterInstance(soundView);
            containerBuilder.RegisterInstance(backgroundView);
            containerBuilder.RegisterInstance(mainView);
            containerBuilder.RegisterInstance(marketView);
            containerBuilder.RegisterInstance(luckyWheelView);
            containerBuilder.RegisterInstance(levelView);
            containerBuilder.RegisterInstance(boostersView);
            containerBuilder.RegisterInstance(infoView);
            containerBuilder.RegisterInstance(questionView);
            containerBuilder.RegisterInstance(world);
            containerBuilder.RegisterInstance(endView);
        }
    }
}