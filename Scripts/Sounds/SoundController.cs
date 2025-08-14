namespace FruityPaw.Scripts.Sounds
{
    public class SoundController
    {
        private readonly SoundView _soundView;
        private readonly SoundConfig _soundConfig;
        
        public bool OnSound { get; private set; }

        public SoundController(SoundView soundView,
            SoundConfig soundConfig)
        {
            _soundView = soundView;
            _soundConfig = soundConfig;
        }

        public void Start()
        {
            OnSound = true;
        }

        public void ChangeOnSound()
        {
            OnSound = !OnSound;
        }

        public void PlaySound(SoundType soundType)
        {
            if (!OnSound) return;
            
            var clip = _soundConfig.GetSoundByType(soundType);
            if (clip != null)
                _soundView.PlayClip(clip);
        }

        public void StopSound()
        {
            _soundView.StopClip();
        }
    }
}