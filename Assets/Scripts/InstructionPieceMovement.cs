using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPieceMovement: MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public float speed;
    public float distance;
    public bool highlight = true;
    public enum Direction { X, Y, Z}
    public Direction direction;

    private float percentageOfMovementElapsed = 0;
    private bool resetting;
    // Start is called before the first frame update
    void Start()
    {
        SetMovementPositions(direction);
        if (false)
        {
            SetHighlight();
        }

    }

    void SetMovementPositions(Direction direction)
    {
        endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        
        switch (direction)
        {
            case Direction.X:
                startPosition = endPosition + new Vector3(distance, 0, 0);
                break;
            case Direction.Y:
                startPosition = endPosition + new Vector3(0, distance, 0);
                break;
            case Direction.Z:
                startPosition = endPosition + new Vector3(0, 0, distance);
                break;
            default:
                break;
        }

        transform.localPosition = startPosition;
    }

    void SetHighlight()
    {
        GameObject highlight = Instantiate(Resources.Load("Highlight")) as GameObject;
        // Make it a child of the parent's parent object (the AR object)
        highlight.transform.SetParent(transform.parent.transform);

        // Set the local position to the local end position of the moving lego piece
        highlight.transform.localPosition = endPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        if (transform.localPosition != endPosition)
        {
            percentageOfMovementElapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, percentageOfMovementElapsed * speed);
        } else
        {
            if (!resetting)
            {
                StartCoroutine(ResetPosition());
            } 
        }   
    }

    IEnumerator ResetPosition()
    {
        resetting = true;
        yield return new WaitForSeconds(1);
        percentageOfMovementElapsed = 0;
        transform.localPosition = startPosition;
        resetting = false;
    }
}
