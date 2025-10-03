// using UnityEngine;
// using TMPro;
// using System.Collections.Generic;

// public class SphereManager : MonoBehaviour
// {
//     public static SphereManager Instance { get; private set; }

//     public SaveData saveDataScript; // Reference to the SaveData script
//     public Player playerScript; // Reference to the Player script
//     public GameObject spherePrefab;
//     public Transform playAreaCenter;
//     private float playAreaSize;
//     public float sphereHeight = 1.8f;
//     public TextMeshProUGUI timerText;
//     public TextMeshProUGUI scoreText;

//     private GameObject currentSphere;
//    // private int currentSphereIndex = 0; // Track sphere index for logging

//     // We no longer need a countdown timer, so we'll use a stopwatch that counts up.
//     private float stopwatchTime = 0f;
//     private const int winCondition = 15; // The number of catches 
//     private int score = 0;
//     private bool gameActive = true;

//     // Trigger cube handling
//     private List<GameObject> triggerObjects = new List<GameObject>();
//     private int roundCount = 0;
//     private int currentArrayIndex = 0;

//     // --- NEW VARIABLES FOR FIXED SPAWN PATH ---
//     // Array to hold the 10 Point GameObjects.
//     // Public to allow manual assignment in the Inspector.
//     public Transform[] spawnPoints; 
//    // private int pointIndex = 0; // Index to track the current spawn point


//     // Array 1
//     private Vector3[] triggerPositions1 = new Vector3[]
//     {
//         new Vector3(-0.95f, 0.10f, -0.93f),
//         new Vector3(-2.12f, 0.10f, 1.51f),
//         new Vector3(-0.01f, 0.10f, 1.99f),
//         new Vector3(0.24f, 0.10f, -1.85f),
//         new Vector3(-1.93f, 0.10f, -2.02f),
//         new Vector3(0.95f, 0.10f, 0.36f),
//         new Vector3(1.99f, 0.10f, 0.70f),
//         new Vector3(-1.54f, 0.10f, 2.27f),
//         new Vector3(1.92f, 0.10f, 0.18f),
//         new Vector3(1.21f, 0.10f, 2.12f),
//         new Vector3(-1.04f, 0.10f, -2.09f),
//         new Vector3(-1.83f, 0.10f, 0.88f),
//         new Vector3(1.07f, 0.10f, -1.41f),
//         new Vector3(-1.92f, 0.10f, 0.03f),
//         new Vector3(0.84f, 0.10f, -2.36f),
//         new Vector3(0.95f, 0.10f, 1.21f),
//         new Vector3(2.04f, 0.10f, 1.66f),
//         new Vector3(0.18f, 0.10f, 0.65f),
//         new Vector3(-2.11f, 0.10f, -0.87f),
//         new Vector3(-0.76f, 0.10f, 1.25f),
//         new Vector3(-0.81f, 0.10f, -0.02f),
//         new Vector3(1.99f, 0.10f, -1.32f),
//         new Vector3(0.98f, 0.10f, -0.40f),
//         new Vector3(0.31f, 0.10f, -0.91f)
//     };

//     // Array 2
//     private Vector3[] triggerPositions2 = new Vector3[] 	{
//         new Vector3(-0.57f, 0.10f, 1.72f),
//         new Vector3(1.11f, 0.10f, 0.06f),
//         new Vector3(-0.01f, 0.10f, 1.99f),
//         new Vector3(-0.09f, 0.10f, -2.17f),
//         new Vector3(-1.93f, 0.10f, -2.02f),
//         new Vector3(-1.75f, 0.10f, 0.85f),
//         new Vector3(-1.97f, 0.10f, -0.84f),
//         new Vector3(-0.82f, 0.10f, -1.61f),
//         new Vector3(1.92f, 0.10f, 0.18f),
//         new Vector3(-1.76f, 0.10f, 1.79f),
//         new Vector3(-1.04f, 0.10f, 0.90f),
//         new Vector3(2.02f, 0.10f, 1.42f),
//         new Vector3(1.77f, 0.10f, 1.93f),
//         new Vector3(-1.92f, 0.10f, 0.03f),
//         new Vector3(0.84f, 0.10f, 1.64f),
//         new Vector3(0.95f, 0.10f, 0.89f),
//         new Vector3(2.04f, 0.10f, 0.77f),
//         new Vector3(0.18f, 0.10f, 0.65f),
//         new Vector3(-1.14f, 0.10f, -0.82f),
//         new Vector3(0.93f, 0.10f, -1.77f),
//         new Vector3(-0.81f, 0.10f, -0.02f),
//         new Vector3(1.99f, 0.10f, -1.21f),
//         new Vector3(0.98f, 0.10f, -0.97f),
//         new Vector3(0.04f, 0.10f, -0.75f)
//     };

//     // Array 3
//     private Vector3[] triggerPositions3 = new Vector3[]
//     {
//         new Vector3(1.05f, 0.10f, -0.95f),
//         new Vector3(-1.12f, 0.10f, 1.59f),
//         new Vector3(0.15f, 0.10f, 1.79f),
//         new Vector3(-0.34f, 0.10f, -1.75f),
//         new Vector3(-1.83f, 0.10f, -1.95f),
//         new Vector3(1.01f, 0.10f, 0.45f),
//         new Vector3(2.09f, 0.10f, 0.80f),
//         new Vector3(-1.64f, 0.10f, 2.34f),
//         new Vector3(1.87f, 0.10f, 0.27f),
//         new Vector3(1.11f, 0.10f, 2.05f),
//         new Vector3(-1.14f, 0.10f, -2.15f),
//         new Vector3(-1.91f, 0.10f, 0.94f),
//         new Vector3(1.15f, 0.10f, -1.33f),
//         new Vector3(-1.88f, 0.10f, 0.11f),
//         new Vector3(0.76f, 0.10f, -2.42f),
//         new Vector3(0.87f, 0.10f, 1.17f),
//         new Vector3(2.14f, 0.10f, 1.74f),
//         new Vector3(0.28f, 0.10f, 0.71f),
//         new Vector3(-2.01f, 0.10f, -0.78f),
//         new Vector3(-0.85f, 0.10f, 1.34f),
//         new Vector3(-0.91f, 0.10f, 0.07f),
//         new Vector3(2.05f, 0.10f, -1.25f),
//         new Vector3(0.88f, 0.10f, -0.48f),
//         new Vector3(0.41f, 0.10f, -0.83f)
//     };

//     // Array 4
//     private Vector3[] triggerPositions4 = new Vector3[]
//     {
//         new Vector3(-0.67f, 0.10f, 1.68f),
//         new Vector3(1.21f, 0.10f, 0.11f),
//         new Vector3(0.09f, 0.10f, 2.02f),
//         new Vector3(-0.15f, 0.10f, -2.24f),
//         new Vector3(-2.03f, 0.10f, -2.08f),
//         new Vector3(-1.65f, 0.10f, 0.79f),
//         new Vector3(-1.89f, 0.10f, -0.92f),
//         new Vector3(-0.92f, 0.10f, -1.53f),
//         new Vector3(2.02f, 0.10f, 0.23f),
//         new Vector3(-1.66f, 0.10f, 1.86f),
//         new Vector3(-0.94f, 0.10f, 0.97f),
//         new Vector3(2.12f, 0.10f, 1.48f),
//         new Vector3(1.87f, 0.10f, 2.03f),
//         new Vector3(-1.82f, 0.10f, 0.09f),
//         new Vector3(0.94f, 0.10f, 1.59f),
//         new Vector3(1.05f, 0.10f, 0.95f),
//         new Vector3(2.14f, 0.10f, 0.83f),
//         new Vector3(0.28f, 0.10f, 0.61f),
//         new Vector3(-1.24f, 0.10f, -0.75f),
//         new Vector3(1.03f, 0.10f, -1.71f),
//         new Vector3(-0.91f, 0.10f, 0.01f),
//         new Vector3(2.09f, 0.10f, -1.17f),
//         new Vector3(1.08f, 0.10f, -0.91f),
//         new Vector3(0.14f, 0.10f, -0.68f)
//     };

//     // Array 5
//     private Vector3[] triggerPositions5 = new Vector3[]
//     {
//         new Vector3(-0.77f, 0.10f, 1.62f),
//         new Vector3(1.31f, 0.10f, 0.17f),
//         new Vector3(0.19f, 0.10f, 2.05f),
//         new Vector3(-0.21f, 0.10f, -2.30f),
//         new Vector3(-2.13f, 0.10f, -2.12f),
//         new Vector3(-1.55f, 0.10f, 0.73f),
//         new Vector3(-1.79f, 0.10f, -1.00f),
//         new Vector3(-1.02f, 0.10f, -1.47f),
//         new Vector3(2.12f, 0.10f, 0.28f),
//         new Vector3(-1.56f, 0.10f, 1.93f),
//         new Vector3(-0.84f, 0.10f, 1.04f),
//         new Vector3(2.22f, 0.10f, 1.54f),
//         new Vector3(1.97f, 0.10f, 2.13f),
//         new Vector3(-1.72f, 0.10f, 0.15f),
//         new Vector3(1.04f, 0.10f, 1.54f),
//         new Vector3(1.15f, 0.10f, 1.01f),
//         new Vector3(2.24f, 0.10f, 0.89f),
//         new Vector3(0.38f, 0.10f, 0.57f),
//         new Vector3(-1.34f, 0.10f, -0.68f),
//         new Vector3(1.13f, 0.10f, -1.65f),
//         new Vector3(-1.01f, 0.10f, 0.05f),
//         new Vector3(2.19f, 0.10f, -1.13f),
//         new Vector3(1.18f, 0.10f, -0.85f),
//         new Vector3(0.24f, 0.10f, -0.61f)
//     };

//     private Vector3[][] allTriggerPositions;

//     void Awake()
//     {
//         if (Instance != null && Instance != this)
//             Destroy(gameObject);
//         else
//             Instance = this;
//     }

//     void Start()
//     {
//         if (saveDataScript == null) saveDataScript = FindObjectOfType<SaveData>();
//         if (playerScript == null) playerScript = FindObjectOfType<Player>();

//         // Get play area size from plane
//         if (playAreaCenter != null)
//         {
//             Vector3 planeScale = playAreaCenter.localScale;
//             playAreaSize = 10f * planeScale.x;
//         }
//         else
//         {
//             Debug.LogError("No Play Area Center assigned!");
//         }

//         // Initialize arrays
//         allTriggerPositions = new Vector3[][]
//         {
//             triggerPositions1,
//             triggerPositions2,
//             triggerPositions3,
//             triggerPositions4,
//             triggerPositions5
//         };

//         // Assign initial trigger positions
//         AssignTriggerPositions(currentArrayIndex);

//         // Set initial timer to 0
//         stopwatchTime = 0f;
        
//         // --- NEW: Check if spawnPoints array is set up and if not, try to find them
//         // if (spawnPoints == null || spawnPoints.Length == 0)
//         // {
//         //     spawnPoints = new Transform[winCondition]; 
//         //     for (int i = 0; i < winCondition; i++)
//         //     {
//         //         GameObject pointObj = GameObject.Find($"Point{i + 1}");
//         //         if (pointObj != null)
//         //         {
//         //             spawnPoints[i] = pointObj.transform;
//         //         }
//         //         else
//         //         {
//         //             Debug.LogError($"Could not find GameObject named 'Point{i + 1}'. Make sure it exists in the scene.");
//         //         }
//         //     }
//         // }
        
//         StartRound();
//     }

//     void Update()
//     {
//         if (!gameActive) return;

//         // Count up the stopwatch time
//         stopwatchTime += Time.deltaTime;
        
//         // Format the stopwatch time for display
//         timerText.text = "Time: " + stopwatchTime.ToString("F2"); // "F2" formats to two decimal places
//         scoreText.text = "Score: " + score.ToString();

//         // No longer need to check gameTimer for game over, as it's now score-based.
//         // The EndGame() call has been moved to TryCatchSphere.
//     }

//     void StartRound()
//     {
//         roundCount++;

//         // Update trigger cube positions every 3 rounds
//         if (roundCount % 3 == 1 && roundCount > 1)
//         {
//             currentArrayIndex = (currentArrayIndex + 1) % allTriggerPositions.Length;
//             AssignTriggerPositions(currentArrayIndex);
//         }

//         if (currentSphere != null)
//             Destroy(currentSphere);

//         // --- NEW SPAWNING LOGIC ---
//         // Check if all rounds are completed
//         // if (pointIndex >= winCondition)
//         // {
//         //     EndGame();
//         //     return;
//         // }

//         // Vector3 spawnPos = GetSpawnPosition();
//         // currentSphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
//         // currentSphereIndex++;
//     }

//     // --- NEW FUNCTION to get the next spawn point ---
//     // Vector3 GetSpawnPosition()
//     // {
//     //     // Get the position from the current point in the array
//     //     Vector3 pointPosition = spawnPoints[pointIndex].position;
//     //       Debug.Log($"Spawning sphere {pointIndex + 1} at Point{pointIndex + 1}: {pointPosition}");
//     //     // Increment the index for the next round
//     //     pointIndex++;

//     //     // Return the new position, keeping the sphere's height constant
//     //     return new Vector3(pointPosition.x, sphereHeight, pointPosition.z);
//     // }
    
    
//     void EndGame()
//     {
//         gameActive = false;

//         if (currentSphere != null)
//             Destroy(currentSphere);

//         Debug.Log("Game Over! Score: " + score + ", Time: " + stopwatchTime.ToString("F2"));
//     }

//     public void TryCatchSphere(GameObject obj)
//     {
//     //     if (!gameActive || obj != currentSphere) return;
//     //     // LOG TARGET EVENT BEFORE UPDATING SCORE
//     //     if (saveDataScript != null && currentSphere != null)
//     //   {
//     //     saveDataScript.LogTargetEvent(currentSphereIndex, currentSphere.transform.position);
//     //   }

//     //     score++;

//         // Check for the win condition after each successful catch
//         // if (score >= winCondition)
//         if(SphereTeleporter.gameOver)
//         {
//             EndGame();
//             return; // Exit the method so a new sphere isn't spawned
//         }

//         Debug.Log("Caught! Score: " + score);
//         StartRound();
//     }

//     void AssignTriggerPositions(int index)
//     {
//         Vector3[] selectedPositions = allTriggerPositions[index];

//         for (int i = 0; i < triggerObjects.Count; i++)
//         {
//             if (i < selectedPositions.Length)
//             {
//                 triggerObjects[i].transform.position = selectedPositions[i];
//             }
//             else
//             {
//                 triggerObjects[i].transform.position = selectedPositions[selectedPositions.Length - 1];
//             }
//         }
//     }
// }