using System;
using System.Collections.Generic;
using BackEnd.Enums;
using BackEnd.Base_Classes;
using Managers;
using UnityEngine;

namespace BackEnd.Utilities
{
    public static class AudioManager
    {

        private const string SfxVolumeKey = "GlobalSfxVolume";
        private const string SoundtrackMuteKey = "SoundtrackMuted";

        public static float GlobalSfxVolume
        {
            get => PlayerPrefs.GetFloat(SfxVolumeKey, 1f); // default to full volume
            set
            {
                PlayerPrefs.SetFloat(SfxVolumeKey, value);
                PlayerPrefs.Save();
            }
        }

        public static bool IsSoundtrackMuted
        {
            get => PlayerPrefs.GetInt(SoundtrackMuteKey, 0) == 1;
            set
            {
                PlayerPrefs.SetInt(SoundtrackMuteKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        

        public static void PlayAudio<TType>(AudioSource audioSource, DataObject<TType> data, SfxType type)
        {
            var audioClip = data.GetSfx(type);
            if (audioClip == null) return;

            var clipVolume = data.GetClipVolume(type);
            var finalVolume = clipVolume * GlobalSfxVolume;
            audioSource.PlayOneShot(audioClip, finalVolume);
        }

        public static void PlayRelevantSoundtrack(AudioSource audioSource, List<LevelSoundTrackEntry> levelsSoundtrack)
        {
            audioSource.Stop();
            ApplySoundtrackToLevel(audioSource, levelsSoundtrack);
            audioSource.Play();
        }


        private static void ApplySoundtrackToLevel(AudioSource audioSource, List<LevelSoundTrackEntry> levelsSoundtrack)
        {
            foreach (var entry in levelsSoundtrack)
            {
                if (entry.Soundtrack == null) return;
                if ((int)entry.Level != LevelLoader.Instance.LevelIndex) continue;
                audioSource.clip = entry.Soundtrack;
                break;
            }
        }




    }
}

