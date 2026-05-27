using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title: 音乐音效管理器
/// Description:
/// </summary>
public class AudioMgr : Singleton<AudioMgr>
{
    private AudioSource BGM = null;
    private float BGMVolume = 0.5f;

    private List<AudioSource> soundList = new();
    //音效音量大小
    private float SoundVolume = 0.3f;
    private bool IsSoundPlay = true;
    private IGameAssetLoader assetLoader = GameAssetLoader.Instance;
    private AudioMgr()
    {
        MonoMgr.Instance.AddFixedUpdateListener(Update);
    }

    private void Update()
    {
        if (!IsSoundPlay)
            return;

        for (int i = soundList.Count - 1; i >= 0; i++)
        {
            if (!soundList[i].isPlaying)
            {
                soundList[i].clip = null;
                PoolManager.Instance.PushGameObject(soundList[i].gameObject);
                soundList.RemoveAt(i);
            }
        }
    }

    public void PlayBGM(string name)
    {
        if (BGM == null)
        {
            GameObject go = new GameObject("BGM");
            GameObject.DontDestroyOnLoad(go);
            BGM = go.AddComponent<AudioSource>();
        }

        assetLoader.LoadPackagedAsset<AudioClip>("music", name, null, AssetPackageMode.EditorResources, (clip) =>
        {
            BGM.clip = clip;
            BGM.loop = true;
            BGM.volume = BGMVolume;
            BGM.Play();
        });
    }

    public void PlayBGM(AudioClip clip, float v)
    {
        if (BGM == null)
        {
            GameObject go = new GameObject("BGM");
            GameObject.DontDestroyOnLoad(go);
            BGM = go.AddComponent<AudioSource>();
        }
        BGM.clip = clip;
        BGM.loop = true;
        BGMVolume = v;
        BGM.volume = BGMVolume;
        BGM.Play();
    }


    public void StopBGM(string name)
    {
        if (BGM == null)
            return;
        BGM.Stop();
    }

    public void PauseBGM(string name)
    {
        if (BGM == null)
            return;
        BGM.Pause();
    }

    public void ChangeBGMValue(float v)
    {
        BGMVolume = v;
        if (BGM == null)
            return;
        BGM.volume = v;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="callBack">播放后的回调</param>
    /// <param name="isAsync">是否异步加载</param>
    public void PlaySound(string name, bool isLoop = false, UnityAction<AudioSource> callBack = null, bool isAsync = false)
    {
        assetLoader.LoadPackagedAsset<AudioClip>("sound", name, null, AssetPackageMode.EditorResources, (clip) =>
        {
            PoolManager.Instance.GetGameObject("Audio", "SoundObj", E_LoadWay.ResourcesLoad, false, (obj) =>
            {
                AudioSource source = obj.GetComponent<AudioSource>();
                source.Stop();
                source.clip = clip;
                source.loop = isLoop;
                source.volume = SoundVolume;
                source.Play();
                if (!soundList.Contains(source))
                    soundList.Add(source);
                callBack?.Invoke(source);
            });
        }, isAsync);
    }

    public void PlaySound(AudioClip clip, bool isLoop, float v)
    {
        PoolManager.Instance.GetGameObject("Audio", "SoundObj", E_LoadWay.ResourcesLoad, false, (obj) =>
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            source.Stop();
            source.clip = clip;
            source.loop = isLoop;
            SoundVolume = v;
            source.volume = SoundVolume;
            source.Play();
            if (!soundList.Contains(source))
                soundList.Add(source);
        });
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="audioSource">音效组件</param>
    public void StopSound(AudioSource audioSource)
    {
        if (soundList.Contains(audioSource))
        {
            audioSource.Stop();
            soundList.Remove(audioSource);
            audioSource.clip = null;
            PoolManager.Instance.PushGameObject(audioSource.gameObject);
        }
    }

    public void ChangeSoundValue(float v)
    {
        SoundVolume = v;
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].volume = SoundVolume;
        }
    }

    /// <summary>
    /// 播放或暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否播放</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                soundList[i].Play();
                IsSoundPlay = true;
            }
        }
        else
        {
            for (int i = 0; i < soundList.Count; i++)
            {
                IsSoundPlay = false;
                soundList[i].Pause();
            }
        }
    }

    public void ClearSound()
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].Stop();
            soundList[i].clip = null;
            PoolManager.Instance.PushGameObject(soundList[i].gameObject);
        }
        soundList.Clear();
    }

}
