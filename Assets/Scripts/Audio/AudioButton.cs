
using System;
using BackEnd.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioButton: MonoBehaviour
    {
        public AudioButtonType Type;
        private Toggle _toggle;
        private void Awake()
        {
            _toggle = GetComponent<Toggle>();

        }

        private void Start()
        {
            SetRelevantToggles();
        }

        private void SetRelevantToggles()
        {
            switch (Type)
            {
                case AudioButtonType.MuteSfx:
                    AudioSettingsUI.Instance.SetSfxTogglesToCurrentAudio(_toggle);
                    break;
                case AudioButtonType.MuteSoundTrack:
                    AudioSettingsUI.Instance.SetSoundtrackToggleToCurrentAudio(_toggle);
                    break;

                default:
                    print($"{Type} is out of range");
                    break;
            }
        }

        public void MuteSfx()
        {
            AudioSettingsUI.Instance.MuteSfx();
        }

        public void MuteSoundtrack()
        {
            AudioSettingsUI.Instance.MuteSoundtrack();
        }
    }
}