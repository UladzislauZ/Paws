using System.Linq;
using UnityEngine;

namespace FruityPaw.Scripts.Sounds
{
    [CreateAssetMenu(menuName = "FruityPaw/Sound Config", fileName = "Sound Config")]
    public class SoundConfig : ScriptableObject
    {
        public SoundItem[] sounds;
        public Sprite onSoundSprite;
        public Sprite offSoundSprite;

        public AudioClip GetSoundByType(SoundType soundType)
        {
            return sounds.FirstOrDefault(x => x.soundType == soundType)?.clip;
        }
    }

    [System.Serializable]
    public class SoundItem
    {
        public SoundType soundType;
        public AudioClip clip;
    }
}