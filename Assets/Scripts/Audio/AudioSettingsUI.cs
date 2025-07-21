
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioSettingsUI : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _soundtrackVolumeSlider;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            OnSFXVolumeChanged();
        }

        public void OnSFXVolumeChanged()
        {
            AudioManager.GlobalSfxVolume = _sfxVolumeSlider.value;
        }

        public void OnSoundtrackVolumeChange()
        {
            AudioManager.GlobalSoundtrackVolume = _soundtrackVolumeSlider.value;
            _audioSource.volume = _soundtrackVolumeSlider.value;
        }
    }
}