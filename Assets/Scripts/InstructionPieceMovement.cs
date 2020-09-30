//Created By Ben Westcott, 2020
using System.Collections;
using UnityEngine;

/// <summary>
/// Script that moves a game object to display an animated instruction
/// </summary>
public class InstructionPieceMovement: MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;

    [Tooltip("The speed of the instruction piece")]
    [SerializeField]
    private float speed;

    [Tooltip("The distance of the instruction piece away from its initial starting position")]
    [SerializeField]
    private float distance;

    public enum Direction { X, Y, Z}

    [Tooltip("The direction that the instruction piece will move")]
    [SerializeField]
    private Direction direction;

    private float percentageOfMovementElapsed = 0;
    private bool resetting;

    // Start is called before the first frame update
    void Start()
    {
        SetMovementPositions(direction);
    }

    /// <summary>
    /// Sets the start and end positions of the instruction piece given the direction of travel
    /// </summary>
    /// <param name="direction">The direction of travel</param>
    void SetMovementPositions(Direction direction)
    {
        //End position is where the piece initially starts on the model
        endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        //Start position set by adding the distance to move to the end position in the corresponding direction
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

        //Set the start position to the instruction piece
        transform.localPosition = startPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        if (transform.localPosition != endPosition)
        {
            percentageOfMovementElapsed += Time.deltaTime;

            //If we are not at the end position then gradually move to the end position
            //from the start position with the given speed
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, percentageOfMovementElapsed * speed);
        } else
        {
            //If we have reached the end position and we are not already resetting the
            //position of the instruction piece then reset it
            if (!resetting)
            {
                StartCoroutine(ResetPosition());
            } 
        }   
    }

    /// <summary>
    /// Resets the position of the instruction piece to the start position
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetPosition()
    {
        resetting = true;
        yield return new WaitForSeconds(1);
        percentageOfMovementElapsed = 0;
        transform.localPosition = startPosition;
        resetting = false;
    }
}
