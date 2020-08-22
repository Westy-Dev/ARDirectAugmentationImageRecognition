using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionManager : MonoBehaviour
{
    //Provide in order of build
    [Tooltip("Provide Folder Name With Model Instructions")]
    public string InstructionFolderName;
    private GameObject[] modelInstructions;
    //The position to instantiate the models in the world
    private Vector3 assemblyInitialScale;
    private Quaternion assemblyInitialRotation;

    [SerializeField]
    private ARTrackedImageManager arTrackingImageManager;

    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private bool guidelines;

    private int currentInstructionIndex;
    private int lastInstructionIndex;
    private GameObject currentModel;

    [SerializeField]
    private UIManager uiManager;
    private void OnEnable()
    {
        arTrackingImageManager.trackedImagesChanged += TrackedImagesChanged;
    }

    private void OnDisable()
    {
        arTrackingImageManager.trackedImagesChanged -= TrackedImagesChanged;
    }
    // Start is called before the first frame update
    void Start()
    {

        currentInstructionIndex = 0;

        uiManager.prevButton.SetActive(false);
        // Instantiates a Prefab located in any Resources
        // folder in your project's Assets folder with the name provided in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            lastInstructionIndex = modelInstructions.Length - 1;

        } else
        {
            Debug.LogError("Cannot Load Instructions - Instructions Not Found in Folder: " + InstructionFolderName,gameObject);
        }

    }

    public void loadNextInstruction()
    {
        if (currentInstructionIndex != lastInstructionIndex)
        {
            currentInstructionIndex++;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    private void updateUIButtons()
    {

        if (currentInstructionIndex == 0)
        {
            uiManager.prevButton.SetActive(false);
        }
        else if (!uiManager.prevButton.activeSelf)
        {
            uiManager.prevButton.SetActive(true);
        }

        if (currentInstructionIndex == lastInstructionIndex)
        {
            uiManager.nextButton.SetActive(false);
        }
        else if (!uiManager.nextButton.activeSelf)
        {
            uiManager.nextButton.SetActive(true);
        }

    }
    private void updateUIText()
    {
        int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
        uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
        uiManager.UpdateStepNumber(currentInstructionIndex + 1);
    }
    public void resetTracking()
    {
        arSession.Reset();
    }

    void TrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        //added, spawn prefab
        foreach (ARTrackedImage image in args.added)
        {
            Debug.Log("Image Tracked");
            currentModel = Instantiate(modelInstructions[currentInstructionIndex], image.transform.position, image.transform.rotation);
            currentModel.transform.SetParent(image.transform);

            if (guidelines)
            {
                toggleGuidelines(currentModel);
            }

            updateUIText();
        }

        foreach (ARTrackedImage image in args.removed)
        {
            if(currentModel != null)
            {
                Destroy(currentModel);
            }       
        }
    }

    private void loadNewModel(int currentInstructionIndex)
    {
        TrackableCollection<ARTrackedImage> arTrackables = arTrackingImageManager.trackables;

        foreach(ARTrackedImage aRTrackedImage in arTrackables)
        {
            Destroy(currentModel);
            currentModel = Instantiate(modelInstructions[currentInstructionIndex], aRTrackedImage.transform.position, aRTrackedImage.transform.rotation);
            currentModel.transform.SetParent(aRTrackedImage.transform);

            if (guidelines)
            {
                toggleGuidelines(currentModel);
            }
            updateUIText();
        }
    }

    private int getNumberOfMovingPartsForInstruction(GameObject currentModel)
    {
        int numberOfMovingPartsForInstruction = 0;
        foreach (Transform part in currentModel.transform)
        {
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();
            if (movementScript != null)
            {
                numberOfMovingPartsForInstruction++;
            }

            if (part.childCount > 0)
            {
                numberOfMovingPartsForInstruction += getNumberOfMovingPartsForInstruction(part.gameObject);
            }
        }
        return numberOfMovingPartsForInstruction;
    }

    public void toggleGuidelines()
    {
        toggleGuidelines(currentModel);
        guidelines = !guidelines;
    }

    public void toggleGuidelines(GameObject currentModel)
    {
        foreach (Transform part in currentModel.transform)
        {
            Guidelines materialChangerScript = part.GetComponent<Guidelines>();
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();

            if (materialChangerScript != null && movementScript == null)
            {
                materialChangerScript.toggleGuidelines();
            }

            if(part.childCount > 0)
            {
                toggleGuidelines(part.gameObject);
            }
        }
    }
}
