using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Public references to your other scripts
    public Player playerScript;
    public SaveData saveDataScript;
    public GameObject Canvas;
    public GameObject lastcanvas;
    
    // Reference to SphereTeleporter
    public SphereTeleporter sphereTeleporterScript;
    
    // Input action for continue button
    public InputAction continueAction;

    // Experiment tracking variables
    private int currentCondition = 1;
    private int totalConditions = 3;
    private int currentTrial = 1;
    private int totalTrialsPerCondition = 1; 
    private int[] conditionOrder = { 2, 3, 1 };

    // Public flags to signal when the player is ready to continue
    public bool isReadyToContinue = false;
    public bool isRecordingData = false;

    private bool isTrialComplete = false; // Flag to signal trial completion

    void Start()
    {
        if (continueAction != null)
        {
            continueAction.Enable();
            continueAction.performed += OnContinuePressed;
        }
        
        StartCoroutine(RunExperiment());
    }

    void OnDestroy()
    {
        if (continueAction != null)
        {
            continueAction.performed -= OnContinuePressed;
            
        }
    }

    // Called when the continue button is pressed
    private void OnContinuePressed(InputAction.CallbackContext context)
    {
        if (!isReadyToContinue) 
        {
            isReadyToContinue = true;
            Debug.Log("A button pressed - ready for next trial");
        }
    }

    private IEnumerator RunExperiment()
    {
        //    for (currentCondition = 1; currentCondition <= totalConditions; currentCondition++)
        //    { if (currentCondition == 1 || currentCondition == 2)
        //         {
        //             playerScript.SetProximityHapticsEnabled(false);
        //         }
        //         else
        //         {
        //             playerScript.SetProximityHapticsEnabled(true);
        //         }

        //         for (currentTrial = 1; currentTrial <= totalTrialsPerCondition; currentTrial++)
        //         {
        //             Debug.Log($"Starting Condition {currentCondition}, Trial {currentTrial}");

        //             isReadyToContinue = false; 
        //             isTrialComplete = false; // Reset trial completion flag

        //             // Stop recording data during the break
        //             isRecordingData = false;

        //             // Only show break screen for trials after the first one
        //             if (!(currentCondition == 1 && currentTrial == 1))
        //             {
        //                 // Start the break routine and wait for player input
        //                 yield return StartCoroutine(StartBreak());

        //                 // Start the new trial in SphereTeleporter (this will show the sphere)
        //                 if (sphereTeleporterScript != null)
        //                 {
        //                     sphereTeleporterScript.StartNewTrial();
        //                 }
        //             }

        //             // Start recording data after the player is ready
        //             StartRecording();

        //             // Wait until the SphereTeleporter script signals completion
        //             yield return new WaitUntil(() => isTrialComplete);

        //             // Stop recording and save data
        //             isRecordingData = false;
        //             saveDataScript.SaveAndClearData(currentCondition, currentTrial);

        // Debug.Log($"Completed Condition {currentCondition}, Trial {currentTrial}");




        //3, 2, 1
        // for (currentCondition = totalConditions; currentCondition >= 1; currentCondition--)
        // {
        //     if (currentCondition == 1 || currentCondition == 2)
        //     {
        //         playerScript.SetProximityHapticsEnabled(false);
        //     }
        //     else
        //     {
        //         playerScript.SetProximityHapticsEnabled(true);
        //     }

        //     for (currentTrial = 1; currentTrial <= totalTrialsPerCondition; currentTrial++)
        //     {
        //         Debug.Log($"Starting Condition {currentCondition}, Trial {currentTrial}");

        //         isReadyToContinue = false;
        //         isTrialComplete = false; // Reset trial completion flag

        //         // Stop recording data during the break
        //         isRecordingData = false;

        //         // Only show break screen for trials after the first one
        //         // The original logic `if (!(currentCondition == 1 && currentTrial == 1))`
        //         // means the break screen is skipped for the very first trial of the *entire* experiment.
        //         // This logic remains unchanged.
        //         if (!(currentCondition == totalConditions && currentTrial == 1))
        //         {
        //             // Start the break routine and wait for player input
        //             yield return StartCoroutine(StartBreak());

        //             // Start the new trial in SphereTeleporter (this will show the sphere)
        //             if (sphereTeleporterScript != null)
        //             {
        //                 sphereTeleporterScript.StartNewTrial();
        //             }
        //         }

        //         // Start recording data after the player is ready
        //         StartRecording();

        //         // Wait until the SphereTeleporter script signals completion
        //         yield return new WaitUntil(() => isTrialComplete);

        //         // Stop recording and save data
        //         isRecordingData = false;
        //         saveDataScript.SaveAndClearData(currentCondition, currentTrial);

        //         Debug.Log($"Completed Condition {currentCondition}, Trial {currentTrial}");
        //     }
        // }

        // Debug.Log("Experiment complete!");






        // Iterate through the custom defined condition order (2, 3, 1)
        foreach (int condition in conditionOrder)
        {
            currentCondition = condition; // Set currentCondition to the value from the array

            // Apply haptics settings based on the current condition
            if (currentCondition == 1 || currentCondition == 2)
            {
                playerScript.SetProximityHapticsEnabled(false);
            }
            else // This will be for currentCondition == 3
            {
                playerScript.SetProximityHapticsEnabled(true);
            }

            for (currentTrial = 1; currentTrial <= totalTrialsPerCondition; currentTrial++)
            {
                Debug.Log($"Starting Condition {currentCondition}, Trial {currentTrial}");

                isReadyToContinue = false;
                isTrialComplete = false; // Reset trial completion flag

                // Stop recording data during the break
                isRecordingData = false;

                // Only show break screen for trials after the very first overall trial.
                // The first condition in the ordered list is conditionOrder[0].
                if (!(currentCondition == conditionOrder[0] && currentTrial == 1))
                {
                    yield return StartCoroutine(StartBreak());
                }

                // ALWAYS call StartNewTrial() on SphereTeleporter at the beginning of *every* trial
                // This ensures SphereTeleporter's internal counters are always reset.
                if (sphereTeleporterScript != null)
                {
                    sphereTeleporterScript.StartNewTrial();
                }

                // Start recording data after the player is ready (and after StartNewTrial has run)
                StartRecording();

                // Wait until the SphereTeleporter script signals completion
                yield return new WaitUntil(() => isTrialComplete);

                // Stop recording and save data
                isRecordingData = false;
                saveDataScript.SaveAndClearData(currentCondition, currentTrial);

                Debug.Log($"Completed Condition {currentCondition}, Trial {currentTrial}");
            }
        }

        Debug.Log("Experiment complete!");
        lastcanvas.SetActive(true);



    }
    
    
    public void StartRecording()
{
    isRecordingData = true;
    // You may also want to reset your start time in SaveData here
    // saveDataScript.startTime = Time.time;
    Debug.Log("Data recording has started.");
}

    // Public method for SphereTeleporter to call when the trial is finished
    public void SetTrialCompleted()
    {
        isTrialComplete = true;
    }
    
    // This coroutine handles the break
    private IEnumerator StartBreak()
    {
        Debug.Log("Trial complete. Take a break and physically recenter. Press the continue button to proceed.");
        
         // Activate the canvas
    if (Canvas != null)
    {
        Canvas.SetActive(true);
    }
    
    // Wait until the player is ready
    yield return new WaitUntil(() => isReadyToContinue);

    // Deactivate the canvas
    if (Canvas != null)
    {
        Canvas.SetActive(false);
    }

    Debug.Log("Break over. Proceeding with the next trial.");
    }
}