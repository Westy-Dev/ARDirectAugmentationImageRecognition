using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ARSessionManager arSessionManager;

    [SerializeField]
    private GameObject debugCanvas;

    [SerializeField]
    private GameObject instructionBackground;

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
}
