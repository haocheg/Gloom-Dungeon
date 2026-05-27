using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Audio/Audio DataBase")]
public class AudioDataBase : ScriptableObject
{
    public List<AudioClipData> player;
    public List<AudioClipData> ui;
    public List<AudioClipData> bgm;

    private Dictionary<string, AudioClipData> clipCollection;

    private void OnEnable()
    {
        clipCollection = new Dictionary<string, AudioClipData>();

        Add(player);
        Add(ui);
        Add(bgm);
    }

    public AudioClipData GetAudioClipData(string audioName)
    {
        if (clipCollection.TryGetValue(audioName, out AudioClipData clipData))
            return clipData;
        return null;
    }

    public AudioClip GetAudioClip(string audioName)
    {
        if (clipCollection.TryGetValue(audioName, out AudioClipData clipData))
            return clipData.GetRandomClip();
        return null;
    }

    private void Add(List<AudioClipData> clipDatas)
    {
        foreach (AudioClipData clipData in clipDatas)
        {
            if (clipData != null && !string.IsNullOrEmpty(clipData.audioName) && !clipCollection.ContainsKey(clipData.audioName))
            {
                clipCollection.Add(clipData.audioName, clipData);
            }
        }
    }
}


[System.Serializable]
public class AudioClipData
{
    public string audioName;
    public List<AudioClip> clips = new List<AudioClip>();
    [Range(0f, 1f)] public float volume = 1f;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Count == 0)
            return null;
        return clips[Random.Range(0, clips.Count)];
    }
}

public class AudioDataInfo
{
    public float SFXVolumn;
    public float BGMVolumn;

    public AudioDataInfo()
    {
        SFXVolumn = 0.6f;
        BGMVolumn = 0.6f;
    }

    public AudioDataInfo(float sfxvolumn, float bgmVolumn)
    {
        this.SFXVolumn = sfxvolumn;
        this.BGMVolumn = bgmVolumn;
    }
}