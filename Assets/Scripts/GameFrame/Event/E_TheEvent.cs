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

    E_PlayerHealthChange,

    E_AddScore,

    E_GetScore,

    E_TransmitScore,

    E_GameOver,

    E_ItemSpawn,

    E_SkillBuy,

    E_PlayerGetSkill,

    E_ChangeScene,

    E_IsEnemyDie,

}
