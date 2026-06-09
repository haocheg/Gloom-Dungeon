using UnityEngine;


/// <summary>
/// Title:事件声明枚举，方便管理和使用
/// Description:
/// </summary>
public enum E_TheEvent
{
    E_DefaultEvent,

    E_SceneLoadProgress,//场景加载进度

    E_PlayerAttackOver,

    E_EnemyAttackOver,

    E_PlayerDeath,

    E_GetPlayerHPPercent,

    E_GameOver,

    E_ItemSpawn,

    E_SkillBuy,

    E_PlayerGetSkill,

    E_ChangeScene,

    E_IsEnemyDie,

    E_GetPlayerHPRequest,

    E_GetPlayerHPResponse,

}
