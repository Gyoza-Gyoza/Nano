using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer; 

    public void AdjustBGMVolume(float sliderValue)
    { 
        mixer.SetFloat("BGM", sliderValue);
    }
    public void AdjustSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SFX", sliderValue);
    }
}
