using System;
using System.Linq;
using FruityPaw.Scripts.Boosters;
using FruityPaw.Scripts.Level.FieldObjects;
using UnityEngine;
using UnityEngine.UI;

namespace FruityPaw.Scripts.Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private StatisticsView botStatisticsView;
        [SerializeField] private StatisticsView playerStatisticsView;
        [SerializeField] private BoosterView[] boosterViews;
        [SerializeField] private GameField[] botPawsFields;
        [SerializeField] private GameField[] playerPawsFields;
        [SerializeField] private GameField[] gameFields;
        [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
        [SerializeField] private RectTransform boostersRectTransform;
        [SerializeField] private RectTransform botStatisticsViewRectTransform;
        [SerializeField] private RectTransform playerStatisticsViewRectTransform;
        [SerializeField] private HorizontalLayoutGroup botHorizontalLayoutGroup;
        [SerializeField] private HorizontalLayoutGroup playerHorizontalLayoutGroup;
        [SerializeField] private RectTransform gameFieldsRectTransform;
        
        public GameField[] BotPawsFields => botPawsFields;
        public GameField[] PlayerPawsFields => playerPawsFields;
        public GameField[] GameFields => gameFields;
        
        public event Action OnClose;
        public event Action<BoosterType> OnBoosterClick;

        public void ResizeToIpad()
        {
            horizontalLayoutGroup.spacing = -300;
            horizontalLayoutGroup.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            boostersRectTransform.anchoredPosition = new Vector2(boostersRectTransform.anchoredPosition.x, -50f);
            botStatisticsViewRectTransform.anchorMin = new Vector2(botStatisticsViewRectTransform.anchorMin.x, 0.96f);
            botStatisticsViewRectTransform.anchorMax = new Vector2(botStatisticsViewRectTransform.anchorMax.x, 0.96f);
            playerStatisticsViewRectTransform.anchorMin = new Vector2(playerStatisticsViewRectTransform.anchorMin.x, 0.96f);
            playerStatisticsViewRectTransform.anchorMax = new Vector2(playerStatisticsViewRectTransform.anchorMax.x, 0.96f);
            botHorizontalLayoutGroup.spacing = -300;
            playerHorizontalLayoutGroup.spacing = -300;
            gameFieldsRectTransform.anchoredPosition = new Vector2(gameFieldsRectTransform.anchoredPosition.x, -35f);
            gameFieldsRectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        public void Initialize()
        {
            foreach (var boosterView in boosterViews)
            {
                boosterView.OnBoosterClick += BoosterClick;
            }
        }

        public void AddFruitToStatistics(Sprite fruitSprite, bool isBot = false)
        {
            if (isBot)
                botStatisticsView.AddFruit(fruitSprite);
            else
                playerStatisticsView.AddFruit(fruitSprite);
        }

        public void UpdateCountBooster(BoosterType boosterType, int boosterCount)
        {
            boosterViews.First(x => x.BoosterType == boosterType).UpdateCount(boosterCount);
        }
        
        public void UpdateBoosterClickable(BoosterType boosterType, bool value)
        {
            boosterViews.First(x => x.BoosterType == boosterType).SetInteractable(value);
        }

        public void ActivateButtons()
        {
            foreach (var boosterView in boosterViews)
            {
                boosterView.Activate();
            }
        }

        public void DeactivateButtons()
        {
            foreach (var boosterView in boosterViews)
            {
                boosterView.Deactivate();
            }
        }
        
        public void Show()
        {
            closeButton.onClick.AddListener(Close);
            InitializeStatistics();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            closeButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }

        private void BoosterClick(BoosterType boosterType)
        {
            OnBoosterClick?.Invoke(boosterType);
        }

        private void Close()
        {
            closeButton.onClick.RemoveAllListeners();
            OnClose?.Invoke();
        }

        private void InitializeStatistics()
        {
            playerStatisticsView.Initialize();
            botStatisticsView.Initialize();
        }
    }
}