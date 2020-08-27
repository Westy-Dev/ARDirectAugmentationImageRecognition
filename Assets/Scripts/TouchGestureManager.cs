using UnityEngine;

/// <summary>
/// Manages the touch-based interactions with the AR model instructions
/// </summary>
public class TouchGestureManager : MonoBehaviour
{
    [Tooltip("The ARCamera for the scene")]
    [SerializeField]
    private Camera arCamera;

    [Tooltip("Movement Sensitivity for touch movement")]
    [SerializeField]
    private float movementSensitivity = 0.01f;

    [Tooltip("Rotate Sensitivity for rotate movement")]
    [SerializeField]
    private float rotateSensitivity = 0.1f;

    [Tooltip("Threshold for activating touch movement when holding touch on the AR model")]
    [SerializeField]
    private float timeHoldingThreshold = 0.3f;

    //Holds a reference to the currently manipulated model
    private GameObject arModel;

    private float timeHolding;
    private bool holding;

    private float initialFingersDistance;
    private Vector3 initialArModelScale;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            scaleModel();
           
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                //Fire a ray from the AR camera at the screen position touched into the AR space
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitARObject;

                //If we hit our AR model get a reference to it
                if (Physics.Raycast(ray, out hitARObject))
                {
                    arModel = hitARObject.transform.gameObject;
                } else
                {
                    arModel = null;
                }
            }

            if(touch.phase == TouchPhase.Stationary)
            {
                //While touch is stationary start adding to the time holding every update cycle
                timeHolding += Time.deltaTime;

                //If we have touched the same position on the AR model for 
                //longer than the threshold and we are not already holding
                if (timeHolding >= timeHoldingThreshold && arModel != null && !holding)
                {
                    //Vibrate the device to indicate the change of interaction mode to 
                    //the user (we are now moving rather than rotating)
                    Vibration.Vibrate(100);
                        holding = true;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                //We've moved so reset time holding
                timeHolding = 0;

                if (arModel != null)
                {
                    //If we are holding then move the model
                    if (holding)
                    {
                        moveModel(touch);
                    }
                    //otherwise rotate it
                    else
                    {
                        rotateModel(touch);
                    }


                }
            }
            //If the finger is removed from the screen then remove our refence to the 
            //AR Model so that we don't move or scale them by accident
            if (touch.phase == TouchPhase.Ended)
            {
                holding = false;
            }
        }
    }
    /// <summary>
    /// Scale functionality for the AR model
    /// </summary>
    private void scaleModel()
    {
        Touch touch1 = Input.touches[0];
        Touch touch2 = Input.touches[1];

        //If we have two touches and either one has just touched
        if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
        {
            //Calculate distance between the two positions is calculated and stored as the initial distance
            initialFingersDistance = Vector2.Distance(touch1.position, touch2.position);

            //Fire a ray from the AR camera at the screen position touched into the AR space
            Ray ray = arCamera.ScreenPointToRay(touch1.position);
            RaycastHit hitARObject;

            //If we hit our AR model get a reference to them and their initial scale
            if (Physics.Raycast(ray, out hitARObject))
            {
                arModel = hitARObject.transform.gameObject;
                initialArModelScale = arModel.transform.localScale;
            }
        }
        // If either touches move we need to start scaling
        else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        {
            //Get the new distance between the two fingers on the screen
            var newFingersDistance = Vector2.Distance(touch1.position, touch2.position);

            //Calculate how much to scale by taking the new distance and dividing it by the old
            var scaleFactor = newFingersDistance / initialFingersDistance;
            if (arModel != null)
            {
                //Apply this scale factor to the AR Instructions
                arModel.transform.localScale = initialArModelScale * scaleFactor;
            }

        }
    }
    /// <summary>
    /// Move functionality for the AR model
    /// </summary>
    /// <param name="touch">The current touch</param>
    private void moveModel(Touch touch)
    {
        //Create a vector based on the touch position movement since last update and the defined sensitivity
        Vector2 touchMovement = (touch.deltaPosition) * movementSensitivity;

        //Apply this vector to the transform of the instructions
        arModel.transform.position = new Vector3(
                    arModel.transform.position.x + touchMovement.x,
                    arModel.transform.position.y + touchMovement.y,
                   arModel.transform.position.z);
    }

    /// <summary>
    /// Rotate functionality for the AR model
    /// </summary>
    /// <param name="touch">The current touch</param>
    private void rotateModel(Touch touch)
    {
         // Create a vector based on the touch position movement since last update
         Vector2 touchRotate = (touch.deltaPosition);

        //Check if the model is upside down or not
        if (Vector3.Dot(arModel.transform.up, Vector3.up) >= 0)
        {
            //Rotate around the local up vector of the ar model by taking the
            //change in position of the touch movement and projecting this onto the camera's right transform
            arModel.transform.Rotate(arModel.transform.up, -Vector3.Dot(touchRotate, Camera.main.transform.right) * rotateSensitivity, Space.World);
        }
        else
        {
            //If we are pointing downwards then rotate the opposite way
            arModel.transform.Rotate(arModel.transform.up, Vector3.Dot(touchRotate, Camera.main.transform.right) * rotateSensitivity, Space.World);
        }
        //Rotate around the camera's right transform by taking the change in position of the touch movement
        //and projecting this onto the camera's up transform
        arModel.transform.Rotate(Camera.main.transform.right, Vector3.Dot(touchRotate, Camera.main.transform.up) * rotateSensitivity, Space.World);
    }
}
