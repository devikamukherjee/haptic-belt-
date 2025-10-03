using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTeleporter : MonoBehaviour
{
    [Header("Game Objects")]
    public Transform player;
    public Transform sphere;
    public Transform[] points;
    public GameObject gameOverCanvas;

    [Header("Audio")]
    public AudioClip catchSound;
    //public AudioClip gameOverSound;
    private AudioSource audioSource;

    [Header("Script References")]
    public SaveData saveDataScript;
    public GameManager gameManager;

    [Header("Trigger Cubes")]
    private List<GameObject> triggerObjects = new List<GameObject>();

    public int currentPointIndex = 0;
    private int roundCount = 0;
    private int currentArrayIndex = 0;

    // Track if sphere should be visible
    private bool sphereActive = true;

    // Trigger cube position arrays
    private Vector3[] triggerPositions1 = new Vector3[]
    {
        new Vector3(-0.95f, 0.10f, -0.93f),
        new Vector3(-2.12f, 0.10f, 1.51f),
        new Vector3(-0.01f, 0.10f, 1.99f),
        new Vector3(0.24f, 0.10f, -1.85f),
        new Vector3(-1.93f, 0.10f, -2.02f),
        new Vector3(0.95f, 0.10f, 0.36f),
        new Vector3(1.99f, 0.10f, 0.70f),
        new Vector3(-1.54f, 0.10f, 2.27f),
        new Vector3(1.92f, 0.10f, 0.18f),
        new Vector3(1.21f, 0.10f, 2.12f),
        new Vector3(-1.04f, 0.10f, -2.09f),
        new Vector3(-1.83f, 0.10f, 0.88f),
        new Vector3(1.07f, 0.10f, -1.41f),
        new Vector3(-1.92f, 0.10f, 0.03f),
        new Vector3(0.84f, 0.10f, -2.36f),
        new Vector3(0.95f, 0.10f, 1.21f),
        new Vector3(2.04f, 0.10f, 1.66f),
        new Vector3(0.18f, 0.10f, 0.65f),
        new Vector3(-2.11f, 0.10f, -0.87f),
        new Vector3(-0.76f, 0.10f, 1.25f),
        new Vector3(-0.81f, 0.10f, -0.02f),
        new Vector3(1.99f, 0.10f, -1.32f),
        new Vector3(0.98f, 0.10f, -0.40f),
        new Vector3(0.31f, 0.10f, -0.91f)
    };

    private Vector3[] triggerPositions2 = new Vector3[]
    {
        new Vector3(-0.57f, 0.10f, 1.72f),
        new Vector3(1.11f, 0.10f, 0.06f),
        new Vector3(-0.01f, 0.10f, 1.99f),
        new Vector3(-0.09f, 0.10f, -2.17f),
        new Vector3(-1.93f, 0.10f, -2.02f),
        new Vector3(-1.75f, 0.10f, 0.85f),
        new Vector3(-1.97f, 0.10f, -0.84f),
        new Vector3(-0.82f, 0.10f, -1.61f),
        new Vector3(1.92f, 0.10f, 0.18f),
        new Vector3(-1.76f, 0.10f, 1.79f),
        new Vector3(-1.04f, 0.10f, 0.90f),
        new Vector3(2.02f, 0.10f, 1.42f),
        new Vector3(1.77f, 0.10f, 1.93f),
        new Vector3(-1.92f, 0.10f, 0.03f),
        new Vector3(0.84f, 0.10f, 1.64f),
        new Vector3(0.95f, 0.10f, 0.89f),
        new Vector3(2.04f, 0.10f, 0.77f),
        new Vector3(0.18f, 0.10f, 0.65f),
        new Vector3(-1.14f, 0.10f, -0.82f),
        new Vector3(0.93f, 0.10f, -1.77f),
        new Vector3(-0.81f, 0.10f, -0.02f),
        new Vector3(1.99f, 0.10f, -1.21f),
        new Vector3(0.98f, 0.10f, -0.97f),
        new Vector3(0.04f, 0.10f, -0.75f)
    };

    private Vector3[] triggerPositions3 = new Vector3[]
    {
        new Vector3(1.05f, 0.10f, -0.95f),
        new Vector3(-1.12f, 0.10f, 1.59f),
        new Vector3(0.15f, 0.10f, 1.79f),
        new Vector3(-0.34f, 0.10f, -1.75f),
        new Vector3(-1.83f, 0.10f, -1.95f),
        new Vector3(1.01f, 0.10f, 0.45f),
        new Vector3(2.09f, 0.10f, 0.80f),
        new Vector3(-1.64f, 0.10f, 2.34f),
        new Vector3(1.87f, 0.10f, 0.27f),
        new Vector3(1.11f, 0.10f, 2.05f),
        new Vector3(-1.14f, 0.10f, -2.15f),
        new Vector3(-1.91f, 0.10f, 0.94f),
        new Vector3(1.15f, 0.10f, -1.33f),
        new Vector3(-1.88f, 0.10f, 0.11f),
        new Vector3(0.76f, 0.10f, -2.42f),
        new Vector3(0.87f, 0.10f, 1.17f),
        new Vector3(2.14f, 0.10f, 1.74f),
        new Vector3(0.28f, 0.10f, 0.71f),
        new Vector3(-2.01f, 0.10f, -0.78f),
        new Vector3(-0.85f, 0.10f, 1.34f),
        new Vector3(-0.91f, 0.10f, 0.07f),
        new Vector3(2.05f, 0.10f, -1.25f),
        new Vector3(0.88f, 0.10f, -0.48f),
        new Vector3(0.41f, 0.10f, -0.83f)
    };

    private Vector3[] triggerPositions4 = new Vector3[]
    {
        new Vector3(-0.67f, 0.10f, 1.68f),
        new Vector3(1.21f, 0.10f, 0.11f),
        new Vector3(0.09f, 0.10f, 2.02f),
        new Vector3(-0.15f, 0.10f, -2.24f),
        new Vector3(-2.03f, 0.10f, -2.08f),
        new Vector3(-1.65f, 0.10f, 0.79f),
        new Vector3(-1.89f, 0.10f, -0.92f),
        new Vector3(-0.92f, 0.10f, -1.53f),
        new Vector3(2.02f, 0.10f, 0.23f),
        new Vector3(-1.66f, 0.10f, 1.86f),
        new Vector3(-0.94f, 0.10f, 0.97f),
        new Vector3(2.12f, 0.10f, 1.48f),
        new Vector3(1.87f, 0.10f, 2.03f),
        new Vector3(-1.82f, 0.10f, 0.09f),
        new Vector3(0.94f, 0.10f, 1.59f),
        new Vector3(1.05f, 0.10f, 0.95f),
        new Vector3(2.14f, 0.10f, 0.83f),
        new Vector3(0.28f, 0.10f, 0.61f),
        new Vector3(-1.24f, 0.10f, -0.75f),
        new Vector3(1.03f, 0.10f, -1.71f),
        new Vector3(-0.91f, 0.10f, 0.01f),
        new Vector3(2.09f, 0.10f, -1.17f),
        new Vector3(1.08f, 0.10f, -0.91f),
        new Vector3(0.14f, 0.10f, -0.68f)
    };

    private Vector3[] triggerPositions5 = new Vector3[]
    {
        new Vector3(-0.77f, 0.10f, 1.62f),
        new Vector3(1.31f, 0.10f, 0.17f),
        new Vector3(0.19f, 0.10f, 2.05f),
        new Vector3(-0.21f, 0.10f, -2.30f),
        new Vector3(-2.13f, 0.10f, -2.12f),
        new Vector3(-1.55f, 0.10f, 0.73f),
        new Vector3(-1.79f, 0.10f, -1.00f),
        new Vector3(-1.02f, 0.10f, -1.47f),
        new Vector3(2.12f, 0.10f, 0.28f),
        new Vector3(-1.56f, 0.10f, 1.93f),
        new Vector3(-0.84f, 0.10f, 1.04f),
        new Vector3(2.22f, 0.10f, 1.54f),
        new Vector3(1.97f, 0.10f, 2.13f),
        new Vector3(-1.72f, 0.10f, 0.15f),
        new Vector3(1.04f, 0.10f, 1.54f),
        new Vector3(1.15f, 0.10f, 1.01f),
        new Vector3(2.24f, 0.10f, 0.89f),
        new Vector3(0.38f, 0.10f, 0.57f),
        new Vector3(-1.34f, 0.10f, -0.68f),
        new Vector3(1.13f, 0.10f, -1.65f),
        new Vector3(-1.01f, 0.10f, 0.05f),
        new Vector3(2.19f, 0.10f, -1.13f),
        new Vector3(1.18f, 0.10f, -0.85f),
        new Vector3(0.24f, 0.10f, -0.61f)
    };

    private Vector3[][] allTriggerPositions;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (saveDataScript == null)
        {
            saveDataScript = FindObjectOfType<SaveData>();
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        allTriggerPositions = new Vector3[][]
        {
            triggerPositions1,
            triggerPositions2,
            triggerPositions3,
            triggerPositions4,
            triggerPositions5
        };

        GameObject[] foundTriggers = GameObject.FindGameObjectsWithTag("trigger");
        triggerObjects.AddRange(foundTriggers);
        Debug.Log($"Found {triggerObjects.Count} trigger objects");

        AssignTriggerPositions(currentArrayIndex);

        // Show the sphere at the first point for the initial trial
        if (points.Length > 0)
        {
            sphere.transform.position = points[currentPointIndex].position;
            sphereActive = true;
            Debug.Log($"Starting at point {currentPointIndex}: {points[currentPointIndex].position}");
        }
    }

    void Update()
    {
        // Only check for sphere collision if sphere is active
        if (sphereActive)
        {
            float distance = Vector3.Distance(player.position, sphere.transform.position);
            if (distance <= 1f)
            {
                OnSphereReached();
            }
        }
    }

    void OnSphereReached()
    {
        if (catchSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(catchSound);
            Debug.Log("Playing catch sound!");
        }
        else
        {
            Debug.LogWarning("Catch sound or AudioSource not set!");
        }

        if (saveDataScript != null)
        {
            saveDataScript.LogSphereReachedEvent(
                roundCount + 1,
                currentPointIndex,
                sphere.position,
                currentArrayIndex + 1
            );
        }

        currentPointIndex++;
        roundCount++;

        Debug.Log($"Sphere reached! Moving to point {currentPointIndex}. Round: {roundCount}");

        if (roundCount % 3 == 1 && roundCount > 1)
        {
            currentArrayIndex = (currentArrayIndex + 1) % allTriggerPositions.Length;
            AssignTriggerPositions(currentArrayIndex);
            Debug.Log($"Updated trigger positions to array {currentArrayIndex + 1}");
        }

        if (currentPointIndex >= points.Length)
        {
            Debug.Log("All spheres reached for this trial.");

            // NEW: Hide the sphere when trial is complete
            HideSphere();

            if (gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true);
            }

            // Signal the GameManager that the trial is complete
            if (gameManager != null)
            {
                gameManager.SetTrialCompleted();
            }
            return;
        }

        sphere.transform.position = points[currentPointIndex].position;
        Debug.Log($"Moved sphere to point {currentPointIndex}: {points[currentPointIndex].position}");
    }

    // Method to hide the sphere
    private void HideSphere()
    {
        sphereActive = false;
        if (sphere != null)
        {
            sphere.gameObject.SetActive(false);
        }
        Debug.Log("Sphere hidden - trial complete");
    }

    // Public method for GameManager to call when starting a new trial
    public void StartNewTrial()
    {
        // Reset counters for new trial
        currentPointIndex = 0;
        roundCount = 0;
        currentArrayIndex = 0;

        // Show the sphere at the first point (like a new sphere appearing)
        ShowSphere();
        
        // Assign new trigger positions
        AssignTriggerPositions(currentArrayIndex);
        
        // Hide game over canvas
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        Debug.Log("New trial started - fresh sphere appeared at point 1");
    }

    // Method to show the sphere (like spawning a new one)
    private void ShowSphere()
    {
        sphereActive = true;
        if (sphere != null)
        {
            sphere.gameObject.SetActive(true);
            if (points.Length > 0)
            {
                sphere.transform.position = points[currentPointIndex].position;
                Debug.Log($"New sphere appeared at point {currentPointIndex}: {points[currentPointIndex].position}");
            }
        }
    }

    void AssignTriggerPositions(int arrayIndex)
    {
        if (triggerObjects.Count == 0)
        {
            Debug.LogWarning("No trigger objects found to position!");
            return;
        }

        Vector3[] selectedPositions = allTriggerPositions[arrayIndex];

        for (int i = 0; i < triggerObjects.Count; i++)
        {
            if (triggerObjects[i] != null)
            {
                if (i < selectedPositions.Length)
                {
                    triggerObjects[i].transform.position = selectedPositions[i];
                }
                else
                {
                    triggerObjects[i].transform.position = selectedPositions[selectedPositions.Length - 1];
                }
            }
        }

        Debug.Log($"Assigned {triggerObjects.Count} trigger objects to positions from array {arrayIndex + 1}");
    }

    public int GetCurrentRound()
    {
        return roundCount;
    }

    public int GetCurrentArrayIndex()
    {
        return currentArrayIndex + 1;
    }
}