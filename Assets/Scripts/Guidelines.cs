using UnityEngine;
/// <summary>
/// Script that controls the guidelines option
/// </summary>
public class Guidelines : MonoBehaviour
{
    private Material defaultMaterial;
    private Material guidelineMaterial;

    public void toggleGuidelines()
    {
        //Load the materials for default and guidelines
        defaultMaterial = Resources.Load("Materials/Invisible", typeof(Material)) as Material;
        guidelineMaterial = Resources.Load("Materials/AlphaOutline", typeof(Material)) as Material;

        //If the material on the object is default
        if (GetComponent<Renderer>().material.name.Contains(defaultMaterial.name))
        {
            //then change it to the guideline material
            GetComponent<Renderer>().material = guidelineMaterial;
        }
        else
        {
            //otherwise change it to the default material
            GetComponent<Renderer>().material = defaultMaterial;
        }
    }

}
