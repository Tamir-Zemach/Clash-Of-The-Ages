using Assets.Scripts.BackEnd.Utilities;
using Assets.Scripts.BackEnd.Enems;
using System.Collections.Generic;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))] 
public class SoundtrackManager : SceneAwareMonoBehaviour<SoundtrackManager>
{
    private AudioSource _audioSource;
    public AudioClip StartMenuSoundtrack;
    public List<LevelSoundTrackEntry> LevelsSoundtrack;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = StartMenuSoundtrack;
        if (StartMenuSoundtrack == null) return;
        _audioSource.Play();
    }

    protected override void InitializeOnSceneLoad()
    {
        if (LevelLoader.Instance.InStartMenu()) return;
        LevelLoader.Instance.OnSceneChanged += PlaySoundtrack;

    }
    private void PlaySoundtrack()
    {
        AudioManager.PlayRelevantSoundtrack(_audioSource, LevelsSoundtrack);
    }

}

