using UnityEngine;
/// <summary>
/// Script that controls highlight flashing on the game object it is attached to
/// </summary>
public class HighlightFlash : MonoBehaviour
{
    [Tooltip("The rate of change between start and end colour")]
    [SerializeField]
    private float speed = 5f;

    [Tooltip("The start colour of the highlight flash")]
    [SerializeField]
    private Color startColor;

    [Tooltip("The end colour of the highlight flash")]
    [SerializeField]
    private Color endColor;

    //Called every fixed-frame update
    private void FixedUpdate()
    {
        //Changes the colour back and forth between the start and end colour
        GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }

}
