using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Dialogue Define/New Database", fileName = "Database - ")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueData> allDialogues;
}
