using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    private SceneMgr() { }


    public void LoadScene(string name, UnityAction callBack = null)
    {
        SceneManager.LoadScene(name);
        callBack?.Invoke();
    }

    public void LoadSceneAsync(string name, UnityAction callBack = null)
    {
        MonoMgr.Instance.StartCoroutine(AtLoadSceneAsnyc(name, callBack));
    }

    private IEnumerator AtLoadSceneAsnyc(string name, UnityAction callBack = null)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while (!ao.isDone)
        {

            EventCenter.Instance.EventTrigger(E_TheEvent.E_SceneLoadProgress, ao.progress);
            yield return 0;
        }
        EventCenter.Instance.EventTrigger<float>(E_TheEvent.E_SceneLoadProgress, 1);
        callBack?.Invoke();
    }

}
