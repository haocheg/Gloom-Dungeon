using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class BGPanel : BasePanel
{
    [SerializeField] private RawImage bg;
    [SerializeField] private Texture t;

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        SetBG(t);
    }

    public void SetBG(Texture texture)
    {
        bg.texture = texture;
    }

}
