using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    [SerializeField] private AudioDataBase dataBase;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    private float sfxVolumn;
    private float bgmVolumn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<AudioManager>();
            if (instance == null)
                instance = gameObject.AddComponent<AudioManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(sfxSource.gameObject);
        DontDestroyOnLoad(bgmSource.gameObject);
        AudioDataInfo dataInfo = JsonMgr.Instance.LoadData<AudioDataInfo>("AudioVolumData");
        sfxVolumn = dataInfo.SFXVolumn;
        bgmVolumn = dataInfo.BGMVolumn;
    }

    public void PlaySFX(string soundName)
    {
        AudioClipData data = dataBase.GetAudioClipData(soundName);
        if (data == null) return;

        AudioClip clip = data.GetRandomClip();
        if (clip == null) return;
        sfxSource.clip = clip;
        sfxSource.volume = sfxVolumn;
        sfxSource.Play();
    }

    public void PlayBGM(string bgmName)
    {
        AudioClipData data = dataBase.GetAudioClipData(bgmName);
        if (data == null) return;

        AudioClip clip = data.GetRandomClip();
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.volume = bgmVolumn;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void ChangeSFXVolumn(float value)
    {
        sfxVolumn = value;
        if (sfxSource != null)
            sfxSource.volume = sfxVolumn;
    }

    public void ChangeBGMVolumn(float value)
    {
        bgmVolumn = value;
        if (bgmSource != null)
            bgmSource.volume = bgmVolumn;
    }

    public void SaveVolumnData()
    {
        AudioDataInfo dataInfo = new AudioDataInfo(sfxVolumn, bgmVolumn);
        JsonMgr.Instance.SaveData(dataInfo, "AudioVolumData");
    }
    public float GetSFXVolumn()
    {
        return sfxVolumn;
    }

    public float GetBGMVolumn()
    {
        return bgmVolumn;
    }


}
