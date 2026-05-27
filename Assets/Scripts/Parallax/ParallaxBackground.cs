using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private ParallaxLayer[] backgrouncLayers;
    [SerializeField] private Camera mainCamera;
    private float lastCameraPosX;
    private float cameraHalfWidth;
    private float imageWidthOffset = 10.0f;

    private void Awake()
    {
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        foreach (ParallaxLayer layer in backgrouncLayers)
        {
            layer.CalcWidth();
        }
    }

    private void FixedUpdate()
    {
        float cameraPosX = mainCamera.transform.position.x;
        float disToMove = cameraPosX - lastCameraPosX;
        lastCameraPosX = cameraPosX;

        float cameraLeftEdge = cameraPosX - cameraHalfWidth + imageWidthOffset;
        float cameraRightEdge = cameraPosX + cameraHalfWidth - imageWidthOffset;

        foreach (ParallaxLayer layer in backgrouncLayers)
        {
            layer.Move(disToMove);
            layer.LoopBackgroud(cameraLeftEdge, cameraRightEdge);
        }
    }

}
