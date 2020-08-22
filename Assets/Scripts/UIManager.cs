using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ARSessionManager arSessionManager;

    [SerializeField]
    private GameObject debugCanvas;

    [SerializeField]
    private Text NumberOfPartsForInstruction;
    [SerializeField]
    private Text StepNumber;

    [SerializeField]
    private Button debugButton;

    [SerializeField]
    private Button guidelinesButton;

    public GameObject prevButton;

    public GameObject nextButton;

    private bool debug = false;
    private bool guidelines = false;
    private Color32 toggleColor = new Color32(159, 159, 159, 255);
    private Color defaultColor = Color.white;

    public void loadNextInstruction()
    {
        arSessionManager.loadNextInstruction();
    }

    public void loadPreviousInstruction()
    {
        arSessionManager.loadPreviousInstruction();
    }

    public void toggleDebug()
    {
        debug = !debug;

        debugCanvas.SetActive(debug);

        if (debug)
        {
            debugButton.image.color = toggleColor;
        }
        else
        {
            debugButton.image.color = defaultColor;
        }
    }

    public void resetTracking()
    {
        arSessionManager.resetTracking();
    }

    public void UpdateNumberOfMovingPartsForInstruction(int numberOfParts)
    {
        NumberOfPartsForInstruction.text = "Parts in Step: " + numberOfParts;
    }

    public void UpdateStepNumber(int stepNumber)
    {
        StepNumber.text = "Step Number: " + stepNumber;
    }

    public void toggleGuidelines()
    {
        guidelines = !guidelines;

        arSessionManager.toggleGuidelines();

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
