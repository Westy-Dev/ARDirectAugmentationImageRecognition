//Created By Ben Westcott, 2020
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages the UI interaction elements. Used for button events.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Tooltip("ARSessionManager to control AR Model Instructions")]
    [SerializeField]
    private ARSessionManager arSessionManager;

    [Tooltip("Debug Canvas to help with debugging during runtime")]
    [SerializeField]
    private GameObject debugCanvas;

    [Tooltip("A text object for number of parts in current instruction display")]
    [SerializeField]
    private Text NumberOfPartsForInstruction;

    [Tooltip("A text object for step number display")]
    [SerializeField]
    private Text StepNumber;

    [Tooltip("Button to toggle the debug canvas on and off")]
    [SerializeField]
    private Button debugButton;

    [Tooltip("Button to toggle the guidelines on and off")]
    [SerializeField]
    private Button guidelinesButton;

    [Tooltip("Button to move to the previous instruction")]
    public GameObject prevButton;

    [Tooltip("Button to move to the next instruction")]
    public GameObject nextButton;

    private bool debug = false;
    private bool guidelines = false;

    private Color32 toggleColor = new Color32(159, 159, 159, 255);
    private Color defaultColor = Color.white;

    /// <summary>
    /// Loads the next instruction using <c>ARSessionManager</c>
    /// </summary>
    public void loadNextInstruction()
    {
        arSessionManager.loadNextInstruction();
    }

    /// <summary>
    /// Loads the previous instruction using <c>ARSessionManager</c>
    /// </summary>
    public void loadPreviousInstruction()
    {
        arSessionManager.loadPreviousInstruction();
    }

    /// <summary>
    /// Toggles the display of the <c>DebugCanvas</c>
    /// </summary>
    public void toggleDebug()
    {
        debug = !debug;

        debugCanvas.SetActive(debug);

        //Sets the colour of the debug button to look like a toggle
        if (debug)
        {
            debugButton.image.color = toggleColor;
        }
        else
        {
            debugButton.image.color = defaultColor;
        }
    }

    /// <summary>
    /// Resets the tracking of the image target using <c>ARSessionManager</c>
    /// </summary>
    public void resetTracking()
    {
        arSessionManager.resetTracking();
    }

    /// <summary>
    ///  Updates the <c>NumberOfPartsForInstruction</c> text element with the given number of parts
    /// </summary>
    /// <param name="stepNumber"> The step number </param>
    public void UpdateNumberOfMovingPartsForInstruction(int numberOfParts)
    {
        NumberOfPartsForInstruction.text = "Parts in Step: " + numberOfParts;
    }

    /// <summary>
    ///  Updates the <c>StepNumber</c> text element with the given step number
    /// </summary>
    /// <param name="stepNumber"> The step number </param>
    public void UpdateStepNumber(int stepNumber)
    {
        StepNumber.text = "Step Number: " + stepNumber;
    }

    /// <summary>
    /// Toggles the guidelines for the current model using the <c>ARSessionManager</c>
    /// </summary>
    public void toggleGuidelines()
    {
        guidelines = !guidelines;

        arSessionManager.toggleGuidelines();

        //Sets the colour of the guidelines button to look like a toggle
        if (guidelines)
        {
            guidelinesButton.image.color = toggleColor;
        }
        else
        {
            guidelinesButton.image.color = defaultColor;
        }
       
    }
}
