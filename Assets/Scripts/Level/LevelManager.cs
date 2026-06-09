using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    public string curLevelName;
    [SerializeField] private int curCheckPoint;
    [SerializeField] private CheckPointInfo checkPointInfo;
    [SerializeField] private List<Transform> itemSpawnPoints;
    private Dictionary<int, string> checkPointDic;
    private CheckPoint[] checkPoints;
    private static int DefaultCheckPoint = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<LevelManager>();
            if (instance == null)
                instance = gameObject.AddComponent<LevelManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        checkPointDic = new Dictionary<int, string>();
        if (checkPointInfo != null && checkPointInfo.pointDatas != null)
        {
            foreach (CheckPointData data in checkPointInfo.pointDatas)
            {
                checkPointDic.Add(data.Index, data.sceneName);
            }
        }
        curCheckPoint = DefaultCheckPoint;
    }

    private void Start()
    {

    }

    public void RespawnPlayer(RespawnType type, UnityAction callBack = null)
    {
        if (!checkPointDic.TryGetValue(curCheckPoint, out string levelName))
        {
            Debug.LogError($"Checkpoint index not found: {curCheckPoint}");
            return;
        }

        if (curLevelName != levelName)
            ChangeScene(levelName, type, callBack);
        else
            ChangeScene(curLevelName, type, callBack);
    }

    public void ChangeScene(string sceneName, RespawnType type, UnityAction callBack = null)
    {
        curLevelName = sceneName;
        StartCoroutine(ChangeSceneCo(curLevelName, type, callBack));
    }


    private IEnumerator ChangeSceneCo(string sceneName, RespawnType type, UnityAction callBack)
    {
        FadePanel fadePanel = null;
        UIManager.Instance.ShowPanel<FadePanel>(E_UILayer.Top, (panel) =>
        {
            fadePanel = panel;
        });

        yield return new WaitUntil(() => fadePanel != null);
        yield return fadePanel.fadeEffectCo;

        SceneMgr.Instance.LoadScene(sceneName);

        yield return new WaitForSeconds(0.5f);

        callBack?.Invoke();
        Vector3 pos = GetRespawnPointPos(type);
        if (pos != Vector3.zero)
            PlayerService.Instance.Teleport(pos);

        yield return null;
        GameManager.Instance.ChangeScene();
        PoolManager.Instance.ClearAllPool();
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.HidePanel<FadePanel>();
        GetFirstCheckPoint();
    }

    public Vector3 GetRespawnPointPos(RespawnType type)
    {
        switch (type)
        {
            case RespawnType.Enter:
            case RespawnType.Exit:
                EventCenter.Instance.EventTrigger(E_TheEvent.E_ChangeScene);
                WayPoint[] point = FindObjectsByType<WayPoint>(FindObjectsSortMode.None);
                foreach (WayPoint p in point)
                {
                    if (p.GetRespawnType() == type)
                        return p.GetRespawnPoint();
                }
                return Vector3.zero;
            case RespawnType.Death:
            case RespawnType.New:
                checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
                foreach (CheckPoint p in checkPoints)
                {
                    if (p.GetPointIndex() == curCheckPoint)
                        return p.GetRespawnPoint();
                }
                return Vector3.zero;
            case RespawnType.None:
            default:
                return Vector3.zero;
        }
    }

    public void SaveCheckPoint(int index)
    {
        if (checkPointDic.ContainsKey(index))
        {
            curCheckPoint = index;
        }
    }

    public void GetFirstCheckPoint()
    {
        checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        if (checkPoints == null || checkPoints.Length == 0)
        {
            Debug.LogWarning("No checkpoint found in current scene.");
            return;
        }

        curCheckPoint = checkPoints[0].GetPointIndex();
    }

    public void Reset()
    {
        curCheckPoint = DefaultCheckPoint;
    }

}
