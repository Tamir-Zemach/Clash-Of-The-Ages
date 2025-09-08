using Audio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.Pause
{
    public class PauseMenu : MonoBehaviour
    {
       public bool MenuOpen;
       public Slider SfxSlider;
       public Slider SoundtrackSlider;
       
       
       public void OnSFXVolumeChanged()
       {
           AudioSettingsUI.Instance.OnSFXVolumeChanged(SfxSlider);
       }

       public void OnSoundtrackVolumeChange()
       {
           AudioSettingsUI.Instance.OnSoundtrackVolumeChange(SoundtrackSlider);
       }

       public void SetSlidersToCurrentAudio()
       {
           AudioSettingsUI.Instance.SetSlidersToCurrentAudio(SfxSlider, SoundtrackSlider);
       }
       
    }
}