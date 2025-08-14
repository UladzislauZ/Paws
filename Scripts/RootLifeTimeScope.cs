using FruityPaw.Scripts.Backgrounds;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Data;
using FruityPaw.Scripts.Info;
using FruityPaw.Scripts.Level;
using FruityPaw.Scripts.Level.Availables;
using FruityPaw.Scripts.LuckyWheel;
using FruityPaw.Scripts.Main;
using FruityPaw.Scripts.Market;
using FruityPaw.Scripts.Popup;
using FruityPaw.Scripts.Sounds;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FruityPaw.Scripts
{
    public class RootLifeTimeScope : LifetimeScope
    {
        [SerializeField] private SceneViews sceneView;
        [SerializeField] private Configs configs;
        
        protected override void Configure(IContainerBuilder builder)
        {
            ControllersConfigure(builder);
            sceneView.Configure(builder);
            configs.Configure(builder);
            builder.Register<LevelModel>(Lifetime.Singleton);
        }

        private void ControllersConfigure(IContainerBuilder builder)
        {
            builder.Register<SoundController>(Lifetime.Singleton);
            builder.Register<BackgroundController>(Lifetime.Singleton);
            builder.Register<DataController>(Lifetime.Singleton);
            builder.Register<MainController>(Lifetime.Singleton);
            builder.Register<InfoController>(Lifetime.Singleton);
            builder.Register<RootController>(Lifetime.Singleton);
            builder.Register<ViewsController>(Lifetime.Singleton);
            builder.Register<LevelViewController>(Lifetime.Singleton);
            builder.Register<MarketController>(Lifetime.Singleton);
            builder.Register<MarketViewController>(Lifetime.Singleton);
            builder.Register<BoostersViewController>(Lifetime.Singleton);
            builder.Register<BoostersController>(Lifetime.Singleton);
            builder.Register<LuckyViewController>(Lifetime.Singleton);
            builder.Register<PopupController>(Lifetime.Singleton);
            builder.Register<LevelController>(Lifetime.Singleton);
            builder.Register<AvailableController>(Lifetime.Singleton);
            builder.Register<PlayerController>(Lifetime.Singleton);
            builder.Register<BotController>(Lifetime.Singleton);
            builder.Register<ActionController>(Lifetime.Singleton);
            builder.Register<SwapBoosterController>(Lifetime.Singleton);
            builder.Register<FreezeBoosterController>(Lifetime.Singleton);
            builder.Register<ExtraMoveBoosterController>(Lifetime.Singleton);
            builder.Register<ComeLineBoosterController>(Lifetime.Singleton);
            builder.Register<ResizeController>(Lifetime.Singleton);
        }

        private void Start()
        {
            Container.Resolve<RootController>().Start();
        }
    }
}
