using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer musicMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("SFXParam", volume);
    }

    public void SetMusic(float music)
    {
        musicMixer.SetFloat("MusicParam", music);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
