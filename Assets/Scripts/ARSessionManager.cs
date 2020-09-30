//Created By Ben Westcott, 2020
using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Manages the position, scale, rotation and display of the <c>CurrentModel</c>
/// </summary>
public class ARSessionManager : MonoBehaviour
{
    //Provide in order of build
    [Tooltip("Provide Folder Name With Model Instruction Prefabs (Prefabs in folder should be in build order)")]
    [SerializeField]
    private string InstructionFolderName;

    //Holds all prefabs for the model instructions
    private GameObject[] modelInstructions;
  
    [Tooltip("ARFoundation ARTrackedImageManager which manages the image tracking and its lifecycle")]
    [SerializeField]
    private ARTrackedImageManager arTrackingImageManager;

    [Tooltip("ARSession which manages the session lifecycle (used to restart the session in this instance)")]
    [SerializeField]
    private ARSession arSession;

    //Toggle to know if guidelines are on or off currently
    private bool guidelines;

    private int currentInstructionIndex;
    private int lastInstructionIndex;

    //The current displayed prefab
    private GameObject currentModel;

    [SerializeField]
    private UIManager uiManager;

    private void OnEnable()
    {
        //Add my defined TrackedImagesChanged function as the event to fire when the tracking manager notices
        //a tracked image change
        arTrackingImageManager.trackedImagesChanged += TrackedImagesChanged;
    }

    private void OnDisable()
    {
        //Remove my defined TrackedImagesChanged function as the event to fire when the tracking manager notices
        //a tracked image change
        arTrackingImageManager.trackedImagesChanged -= TrackedImagesChanged;
    }
    // Start is called before the first frame update
    void Start()
    {

        currentInstructionIndex = 0;

        //Hide previous button on UI
        uiManager.prevButton.SetActive(false);

        //Loads all model instruction prefabs from resources folder using the given instruction folder name in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        //As the resources load all function does not load in order, we need to sort the prefabs by name to get them in order
        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            //Sets the last instruction index
            lastInstructionIndex = modelInstructions.Length - 1;

        } else
        {
            Debug.LogError("Cannot Load Instructions - Instructions Not Found in Folder: " + InstructionFolderName,gameObject);
        }

    }

    /// <summary>
    /// Loads the next instruction from <c>modelInstructions</c>
    /// </summary>
    public void loadNextInstruction()
    {
        if (currentInstructionIndex != lastInstructionIndex)
        {
            currentInstructionIndex++;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    /// <summary>
    /// Loads the previous instruction from <c>modelInstructions</c>
    /// </summary>
    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }

        updateUIButtons();
    }

    /// <summary>
    /// Controls the visibility of the previous and next buttons on the UI
    /// </summary>
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

    /// <summary>
    /// Updates the number of parts per step and step number on the UI
    /// </summary>
    private void updateUIText()
    {
        int numberOfMovingPartsForInstruction = getNumberOfMovingPartsForInstruction(currentModel);
        uiManager.UpdateNumberOfMovingPartsForInstruction(numberOfMovingPartsForInstruction);
        uiManager.UpdateStepNumber(currentInstructionIndex + 1);
    }

    /// <summary>
    /// Resets the tracking of the preset image by resetting the <c>ARSession</c> in progress
    /// </summary>
    public void resetTracking()
    {
        arSession.Reset();
    }

    /// <summary>
    /// Controls what occurs when a tracked image change event is fired by the <c>ARTrackedImageManager</c>
    /// Instantiates the current model at the tracked images position
    /// </summary>
    /// <param name="args"></param>
    void TrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        //If an image is added, spawn the current model instructions to show
        //given by the currentInstructionIndex at the tracked images position and rotation
        foreach (ARTrackedImage image in args.added)
        {
            Debug.Log("Image Tracked");
            currentModel = Instantiate(modelInstructions[currentInstructionIndex],
                            image.transform.position, image.transform.rotation);

            currentModel.transform.SetParent(image.transform);

            if (guidelines)
            {
                //turn on guidelines if they were on before the new image detection
                toggleGuidelines(currentModel);
            }

            updateUIText();
        }

        //If the image is removed from the ARTrackedImageManager then destroy the current model instructions
        foreach (ARTrackedImage image in args.removed)
        {
            if(currentModel != null)
            {
                Destroy(currentModel);
            }       
        }
    }

    /// <summary>
    /// Loads the new model instruction from <c>modelInstructions</c> given the instruction index to load at the position/rotation of the <c>ARTrackedImage</c> transform
    /// </summary>
    /// <param name="currentInstructionIndex">The instruction index number of the instruction to load</param>
    private void loadNewModel(int currentInstructionIndex)
    {
        //Gets all the currently tracked images
        TrackableCollection<ARTrackedImage> arTrackables = arTrackingImageManager.trackables;

        //For each image (in our case this will always be one)
        foreach(ARTrackedImage aRTrackedImage in arTrackables)
        {
            //Destroy the current model
            Destroy(currentModel);
            
            //Loads the next model at the tracked images position and rotation
            currentModel = Instantiate(modelInstructions[currentInstructionIndex],
                            aRTrackedImage.transform.position, aRTrackedImage.transform.rotation);
            
            //Set the model's parent to be the tracked image so that when the image moves, the model will too
            currentModel.transform.SetParent(aRTrackedImage.transform);

            if (guidelines)
            {
                //turn on guidelines if they are still toggled on
                toggleGuidelines(currentModel);
            }
            updateUIText();
        }
    }

    /// <summary>
    /// Recursively obtains the number of moving parts for the current instruction by checking all game objects 
    /// (and child objects) in the current instruction for the <c>InstructionPieceMovement</c> script
    /// </summary>
    /// <param name="currentModel"></param>
    /// <returns>The number of moving parts for the current instruction model</returns>
    private int getNumberOfMovingPartsForInstruction(GameObject currentModel)
    {
        int numberOfMovingPartsForInstruction = 0;

        // checks every part in the current model to see if it has a movement script attached
        foreach (Transform part in currentModel.transform)
        {
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();
            if (movementScript != null)
            {
                //If the part has a movement script, it is moving and therefore a step in the current instruction
                numberOfMovingPartsForInstruction++;
            }

            if (part.childCount > 0)
            {
                //Recursively check the child objects
                numberOfMovingPartsForInstruction += getNumberOfMovingPartsForInstruction(part.gameObject);
            }
        }
        return numberOfMovingPartsForInstruction;
    }

    /// <summary>
    /// Toggles the guideline help for real model positioning
    /// </summary>
    public void toggleGuidelines()
    {
        toggleGuidelines(currentModel);
        guidelines = !guidelines;
    }

    /// <summary>
    /// Toggles the guideline help for for real model positioning
    /// </summary>
    /// <param name="currentModel"></param>
    public void toggleGuidelines(GameObject currentModel)
    {
        // Check all parts in the curent model
        foreach (Transform part in currentModel.transform)
        {
            Guidelines guidelineScript = part.GetComponent<Guidelines>();
            InstructionPieceMovement movementScript = part.GetComponent<InstructionPieceMovement>();

            //If the part does not have a movement script (and is therefore not a new part to put on the real model for this current instruction)
            //and the part has a guideline script on it, then toggle guidelines on this part to show how to position the real model
            if (guidelineScript != null && movementScript == null)
            {
                guidelineScript.toggleGuidelines();
            }

            if(part.childCount > 0)
            {
                //Do this recursively
                toggleGuidelines(part.gameObject);
            }
        }
    }
}
