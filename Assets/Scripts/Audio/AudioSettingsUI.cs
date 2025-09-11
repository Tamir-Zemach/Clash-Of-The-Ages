
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
            _audioSource.mute = AudioManager.IsSoundtrackMuted;
        }
        

        public void SetSfxTogglesToCurrentAudio(Toggle sfxToggle)
        {
            sfxToggle.SetIsOnWithoutNotify(AudioManager.GlobalSfxVolume > 0);
        }

        public void SetSoundtrackToggleToCurrentAudio(Toggle soundtrackToggle)
        {
            soundtrackToggle.SetIsOnWithoutNotify(AudioManager.IsSoundtrackMuted);
        }
        
        public void MuteSfx()
        {
            AudioManager.GlobalSfxVolume = AudioManager.GlobalSfxVolume > 0 ? 0f : 1f;
        }

        public void MuteSoundtrack()
        {
            var newMuteState = !AudioManager.IsSoundtrackMuted;
            AudioManager.IsSoundtrackMuted = newMuteState;
            _audioSource.mute = newMuteState;
        }
    }
}