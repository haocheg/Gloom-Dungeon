using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMulti;
    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalcWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = 0.5f * imageFullWidth;
    }

    public void Move(float dis)
    {
        background.position += Vector3.right * (dis * parallaxMulti);
    }

    public void LoopBackgroud(float leftEdge, float rightEdge)
    {
        float imageLeftEdge = background.position.x - imageHalfWidth;
        float imageRightEdge = background.position.x + imageHalfWidth;

        if (imageRightEdge < leftEdge)
        {
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > rightEdge)
        {
            background.position += Vector3.left * imageFullWidth;
        }
    }

}
