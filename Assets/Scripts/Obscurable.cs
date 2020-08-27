using UnityEngine;
/// <summary>
/// Script which manipulates the render queue of the game object it is attached to.
/// Used for occlusion of the AR moving instructions against the real object.
/// </summary>
public class Obscurable : MonoBehaviour
{
    [Tooltip("Position to render in render queue:" +
        " Background   = 1000 Geometry = 2000 AlphaTest = 2450 Transparent = 3000 Overlay = 4000")]
    public int RendererQueuePosition;
    void Start()
    {
        //Get the renderer for all components of the AR object
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            //Set each components' render queue position
            r.material.renderQueue = RendererQueuePosition;
        }
    }
}
