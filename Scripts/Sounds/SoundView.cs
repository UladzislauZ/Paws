using UnityEngine;

namespace FruityPaw.Scripts.Sounds
{
    public class SoundView : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayClip(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void StopClip()
        {
            audioSource.Stop();
        }
    }
}