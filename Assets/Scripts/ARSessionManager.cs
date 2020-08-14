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
    public Vector3 AssemblyPosition;
    private Vector3 assemblyInitialScale;
    private Quaternion assemblyInitialRotation;

    [SerializeField]
    private ARTrackedImageManager arTrackingImageManager;

    [SerializeField]
    private ARSession arSession;

    private int currentInstructionIndex;
    private int lastInstructionIndex;
    private GameObject currentModel;

    private void OnEnable()
    {
        Debug.Log("Enabled");
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

        // Instantiates a Prefab located in any Resources
        // folder in your project's Assets folder with the name provided in the inspector
        modelInstructions = Resources.LoadAll<GameObject>(InstructionFolderName);

        Array.Sort(modelInstructions, delegate (GameObject x, GameObject y) { return int.Parse(x.name).CompareTo(int.Parse(y.name)); });

        if (modelInstructions !=null || modelInstructions.Length > 0)
        {
            lastInstructionIndex = modelInstructions.Length - 1;

           // arTrackingImageManager.trackedImagePrefab = modelInstructions[currentInstructionIndex];
            //-0.048f, -0.038f, 0.287f
            //currentModel.transform.position = AssemblyPosition;
            //assemblyInitialScale = currentModel.transform.localScale;
            //assemblyInitialRotation = currentModel.transform.rotation;
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
    }

    public void loadPreviousInstruction()
    {
        if (currentInstructionIndex != 0)
        {
            currentInstructionIndex--;
            loadNewModel(currentInstructionIndex);
        }
    }

    public void resetPosition()
    {
        currentModel.transform.localPosition = AssemblyPosition;
        currentModel.transform.localScale = assemblyInitialScale;
        currentModel.transform.rotation = assemblyInitialRotation;
    }

    void TrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        //added, spawn prefab
        foreach (ARTrackedImage image in args.added)
        {
            Debug.Log("Image Tracked");
            currentModel = Instantiate(modelInstructions[currentInstructionIndex], image.transform.position, image.transform.rotation);
            currentModel.transform.SetParent(image.transform);
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
        }
    }
}
