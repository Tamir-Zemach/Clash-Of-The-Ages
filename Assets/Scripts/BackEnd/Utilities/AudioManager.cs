using System;
using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.BackEnd.Utilities;
using BackEnd.Base_Classes;
using Managers;
using UnityEngine;

namespace BackEnd.Utilities
{
    public static class AudioManager
    {

        public static float GlobalSfxVolume;
        public static float GlobalSoundtrackVolume;
        
        

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

