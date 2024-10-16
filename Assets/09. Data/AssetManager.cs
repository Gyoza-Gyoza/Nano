using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AssetManager
{
    public static void LoadVoiceover(string audioURL, Action<AudioClip> onLoaded)
    {
        Addressables.LoadAssetAsync<AudioClip>($"Assets/06. Audio/SFX/Voiceovers/{audioURL}.wav").Completed += (loadedAudio) =>
        {
            onLoaded?.Invoke(loadedAudio.Result);
        };
    }

    public static void SetAudio(string audioURL, GameObject go)
    {
        LoadVoiceover(audioURL, (AudioClip aud) =>
        {
            go.GetComponent<AudioSource>().clip = aud;
        });
    }
}
