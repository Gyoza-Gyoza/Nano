using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AssetManager
{
    public static void LoadAudio(string audioURL, Action<AudioClip> onLoaded)
    {
        Addressables.LoadAssetAsync<AudioClip>(Application.streamingAssetsPath + "/Audio/" + audioURL).Completed += (loadedAudio) =>
        {
            onLoaded?.Invoke(loadedAudio.Result);
        };
    }

    public static void SetAudio(string audioURL, GameObject go)
    {
        LoadAudio(audioURL, (AudioClip aud) =>
        {
            go.GetComponent<AudioSource>().clip = aud;
        });
    }
}
