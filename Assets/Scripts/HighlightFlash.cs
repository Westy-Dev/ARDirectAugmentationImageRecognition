using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightFlash : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    private void FixedUpdate()
    {
        //color.a = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time * speed, 1));

        //GetComponent<Renderer>().material.color = color;

        GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }

}
