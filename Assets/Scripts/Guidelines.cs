using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guidelines : MonoBehaviour
{
    private Material defaultMaterial;
    private Material guidelineMaterial;

    public void toggleGuidelines()
    {
        defaultMaterial = Resources.Load("Materials/Invisible", typeof(Material)) as Material;
        guidelineMaterial = Resources.Load("Materials/AlphaOutline", typeof(Material)) as Material;

        Debug.Log("Default Material = " + defaultMaterial);
        if (GetComponent<Renderer>().material.name.Contains(defaultMaterial.name))
        {
            Debug.Log("Applying guidelines");
            GetComponent<Renderer>().material = guidelineMaterial;
        }
        else
        {
            Debug.Log("Applying default material");
            GetComponent<Renderer>().material = defaultMaterial;
        }
    }

}
