using Cysharp.Threading.Tasks;
using FruityPaw.Scripts.Sounds;
using UnityEngine;

namespace FruityPaw.Scripts.Popup
{
    public class PopupController
    {
        private readonly QuestionView _questionView;
        private readonly EndView _endView;
        private readonly SoundController _soundController;

        private UniTaskCompletionSource<bool> _tcs;

        public PopupController(QuestionView questionView,
            EndView endView,
            SoundController soundController)
        {
            _questionView = questionView;
            _endView = endView;
            _soundController = soundController;
        }
        
        public async UniTask<ViewType> ShowEndPopup(bool isWin, int countReward, Sprite[] rewardSprites)
        {
            _soundController.PlaySound(isWin ? SoundType.Win : SoundType.Lose);
            var result = await _endView.Show(isWin, countReward, rewardSprites);
            _soundController.PlaySound(SoundType.Click);
            return result;
        }

        public async UniTask<bool> ShowQuestionPopupAsync(
            string question,
            string firstTextButton,
            string secondTextButton)
        {
            _tcs = new UniTaskCompletionSource<bool>();
            _questionView.OnButtonPressed += ButtonPressed;
            _questionView.Show(question, firstTextButton, secondTextButton);
            await _tcs.Task;
            _questionView.Hide();
            return _tcs.GetResult(0);
        }

        private void ButtonPressed(bool isYes)
        {
            _questionView.OnButtonPressed -= ButtonPressed;
            _soundController.PlaySound(SoundType.Click);
            _tcs.TrySetResult(isYes);
        }
    }
}