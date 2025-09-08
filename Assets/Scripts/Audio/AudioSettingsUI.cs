
using System;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioSettingsUI : SingletonMonoBehaviour<AudioSettingsUI>
    {
        private AudioSource _audioSource;


        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }
        

        public void OnSFXVolumeChanged(Slider sfxSlider)
        {
            AudioManager.GlobalSfxVolume = sfxSlider.value;
        }

        public void OnSoundtrackVolumeChange(Slider soundtrackSlider)
        {
            AudioManager.GlobalSoundtrackVolume = soundtrackSlider.value;
            _audioSource.volume = soundtrackSlider.value;
        }

        public void SetSlidersToCurrentAudio(Slider sfxSlider, Slider soundtrackSlider)
        {
            sfxSlider.value = AudioManager.GlobalSfxVolume;
            soundtrackSlider.value = AudioManager.GlobalSoundtrackVolume;
        }
        
        public void MuteSfx()
        {
            _audioSource.mute = !_audioSource.mute;
        }
        
    }
}