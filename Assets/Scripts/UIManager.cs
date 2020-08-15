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
    private GameObject instructionBackground;

    [SerializeField]
    private Text NumberOfPartsForInstruction;

    private bool debug = false;
    private bool showInstructionBackground = false;
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
    }

    public void toggleInstructionBackground()
    {
        showInstructionBackground = !showInstructionBackground;

        instructionBackground.SetActive(showInstructionBackground);
    }

    public void resetPosition()
    {
        arSessionManager.resetPosition();
    }

    public void UpdateNumberOfMovingPartsForInstruction(int numberOfParts)
    {
        NumberOfPartsForInstruction.text = "Parts in Step: " + numberOfParts;
    }
}
