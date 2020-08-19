using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obscurable : MonoBehaviour
{
    public int RendererQueuePosition;
    // Start is called before the first frame update
    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material.renderQueue = RendererQueuePosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
