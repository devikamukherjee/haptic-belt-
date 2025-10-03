using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.InputSystem;

// Class to hold all data for a single trial
[System.Serializable]
public class TrialData
{
    // --- Trial Metadata ---
    public List<string> trialInfo = new List<string>();
    public List<string> participantNo = new List<string>();
    public List<string> condition = new List<string>();

    // --- Frame-by-frame Player Data ---
    public List<float> timestamps = new List<float>();
    public List<float> playerPositions_X = new List<float>();
    public List<float> playerPositions_Y = new List<float>();
    public List<float> playerPositions_Z = new List<float>();
    public List<float> playerForwards_X = new List<float>();
    public List<float> playerForwards_Y = new List<float>();
    public List<float> playerForwards_Z = new List<float>();
    public List<float> playerVelocities = new List<float>();
    
    // --- SphereTeleporter (frame-by-frame) ---
    public List<int> roundNumbers = new List<int>();
    public List<int> sphereNumbers = new List<int>();
    public List<float> spherePos_X = new List<float>();
    public List<float> spherePos_Y = new List<float>();
    public List<float> spherePos_Z = new List<float>();
    public List<int> cubeArrayNumbers = new List<int>();
    public List<int> playerCaughtSphere = new List<int>(); // 0/1 per frame

    // --- Event logs ---
    public List<SphereReachedLogEntry> sphereReachedLogs = new List<SphereReachedLogEntry>();
    public List<CollisionLogEntry> collisionLogs = new List<CollisionLogEntry>();
    public List<ProximityLogEntry> proximityLogs = new List<ProximityLogEntry>();


    public void ClearData()
    {
        timestamps.Clear();
        playerPositions_X.Clear(); playerPositions_Y.Clear(); playerPositions_Z.Clear();
        playerForwards_X.Clear();  playerForwards_Y.Clear();  playerForwards_Z.Clear();
        playerVelocities.Clear();

        roundNumbers.Clear();
        sphereNumbers.Clear();
        spherePos_X.Clear(); spherePos_Y.Clear(); spherePos_Z.Clear();
        cubeArrayNumbers.Clear();
        playerCaughtSphere.Clear();

        sphereReachedLogs.Clear();
        collisionLogs.Clear();
        proximityLogs.Clear();

        trialInfo.Clear();
        participantNo.Clear();
        condition.Clear();
    }
}


[System.Serializable]
public class SphereReachedLogEntry
{
    public float timestamp;
    public int roundNumber;       
    public int sphereNumber;     
    public Vector3 spherePosition;
    public int cubeArrayNumber;  
    public bool playerCaught;
}

[System.Serializable]
public class CollisionLogEntry
{
    public float timestamp;
    public int obstacleIndex;
    public Vector3 obstaclePosition;
    public float angle;
    public float width;
    public float depth;
    public float velVal;
    public float speaker1Id;       
    public float speaker1Volume;  
    public float speaker2Id;       
    public float speaker2Volume;
}

[System.Serializable]
public class ProximityLogEntry
{
    public float timestamp;
    public int closestIndex;
    public Vector3 closestPosition;
    public float closestAngle;
    public float closestWidth;
    public float closestDepth;
    public float closestDistance;
    public SpeakerOutput closestSpeaker1;
    public SpeakerOutput closestSpeaker2;
    
    public int secondClosestIndex;
    public Vector3 secondClosestPosition;
    public float secondClosestAngle;
    public float secondClosestWidth;
    public float secondClosestDepth;
    public float secondClosestDistance;
    public SpeakerOutput secondClosestSpeaker1;
     public SpeakerOutput secondClosestSpeaker2;
}

public class SaveData : MonoBehaviour
{
    public TrialData trialData;

    [Header("Metadata")]
    public string participantNo = "P001";
    public string condition = "Standard";
    public string savePath = "Data";

    [Header("References (assign in Inspector)")]
    public Player playerScript;
    public SphereTeleporter sphereTeleporterScript;
    public GameManager gameManager;
    private CharacterController characterController; 
    
    private float startTime;
    private float elapsedTime;
    private int playerCaughtSphereVal = 0;
    private SphereReachedLogEntry lastSphereEvent;

    private void Awake()
    {
        trialData = new TrialData();

        if (playerScript == null)
            playerScript = FindObjectOfType<Player>();

        if (playerScript != null)
            characterController = playerScript.GetComponent<CharacterController>();

        if (sphereTeleporterScript == null)
            sphereTeleporterScript = FindObjectOfType<SphereTeleporter>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }
    
    // Fixed: OnEnable and OnDisable methods removed, as they're now obsolete
    
    private void Start()
    {
        if (sphereTeleporterScript == null)
            Debug.LogError("SaveData: SphereTeleporter reference is missing. Frame-by-frame sphere fields will stay empty.");

        if (playerScript == null)
            Debug.LogError("SaveData: Player reference is missing.");

        startTime = Time.time;
    }

    private void Update()
    {
        elapsedTime = Time.time - startTime;

        if (sphereTeleporterScript != null && sphereTeleporterScript.sphere != null && playerScript != null)
        {
            float d = Vector3.Distance(playerScript.transform.position, sphereTeleporterScript.sphere.position);
            playerCaughtSphereVal = (d <= 1f) ? 1 : 0;
        }

        if (playerScript != null)
        {
            if (characterController != null) // Fixed: Check if characterController is not null
            {
                var p = characterController.transform.position;
                var f = characterController.transform.forward;

                trialData.timestamps.Add(elapsedTime);
                trialData.playerPositions_X.Add(p.x);
                trialData.playerPositions_Y.Add(p.y);
                trialData.playerPositions_Z.Add(p.z);
                trialData.playerForwards_X.Add(f.x);
                trialData.playerForwards_Y.Add(f.y);
                trialData.playerForwards_Z.Add(f.z);
                
                trialData.playerVelocities.Add(playerScript.velVal);

            if (sphereTeleporterScript != null && sphereTeleporterScript.sphere != null && gameManager != null && gameManager.isRecordingData)
                {
                    if (lastSphereEvent != null)
                    {
                        trialData.roundNumbers.Add(lastSphereEvent.roundNumber);
                        trialData.sphereNumbers.Add(lastSphereEvent.sphereNumber);
                        trialData.spherePos_X.Add(lastSphereEvent.spherePosition.x);
                        trialData.spherePos_Y.Add(lastSphereEvent.spherePosition.y);
                        trialData.spherePos_Z.Add(lastSphereEvent.spherePosition.z);
                        trialData.cubeArrayNumbers.Add(lastSphereEvent.cubeArrayNumber);
                    }
                    else
                    {
                        trialData.roundNumbers.Add(0);
                        trialData.sphereNumbers.Add(0);
                        Vector3 sp = sphereTeleporterScript.sphere.position;
                        trialData.spherePos_X.Add(sp.x);
                        trialData.spherePos_Y.Add(sp.y);
                        trialData.spherePos_Z.Add(sp.z);
                        trialData.cubeArrayNumbers.Add(1);
                    }

                    trialData.playerCaughtSphere.Add(playerCaughtSphereVal);
                }

                // Fixed: Metadata is now passed from GameManager, so this is removed
                // trialData.trialInfo.Add("Trial " + trialNumber);
                // trialData.participantNo.Add(participantNo);
                // trialData.condition.Add(condition);
            }
        }
    }

    // Called by SphereTeleporter when the player reaches a sphere
    public void LogSphereReachedEvent(int roundNumber, int sphereNumber, Vector3 spherePosition, int cubeArrayNumber)
    {
        var logEntry = new SphereReachedLogEntry
        {
            timestamp = elapsedTime,
            roundNumber = roundNumber+1,
            sphereNumber = sphereNumber+1,
            spherePosition = spherePosition,
            cubeArrayNumber = cubeArrayNumber,
            playerCaught = true
        };
        
        trialData.sphereReachedLogs.Add(logEntry);
        
        lastSphereEvent = logEntry;

        Debug.Log($"[SaveData] Logged sphere reached: round={roundNumber}, sphere={sphereNumber}, array={cubeArrayNumber}");
    }

   public void LogCollisionEvent(int obstacleIndex, Vector3 obstaclePosition, float angle, 
    float width, float depth, float velVal, object s1, object s2)
{
    var speaker1 = (SpeakerOutput)s1;
    var speaker2 = (SpeakerOutput)s2;
    
    trialData.collisionLogs.Add(new CollisionLogEntry
    {
        timestamp = elapsedTime,
        obstacleIndex = obstacleIndex,
        obstaclePosition = obstaclePosition,
        angle = angle,
        width = width,
        depth = depth,
        velVal = velVal,
        speaker1Id = speaker1.SpeakerId,
        speaker1Volume = speaker1.Volume,
        speaker2Id = speaker2.SpeakerId,
        speaker2Volume = speaker2.Volume
    });
        
    Debug.Log($"[SaveData] Logged collision: index={obstacleIndex}, pos={obstaclePosition}, angle={angle}");
}

public void LogProximityEvent(
    int closestIndex, Vector3 closestPosition, float closestAngle, float closestWidth, float closestDepth, float closestDistance, object closestS1, object closestS2, // Changed to 'object'
    int secondClosestIndex, Vector3 secondClosestPosition, float secondClosestAngle, float secondClosestWidth, float secondClosestDepth, float secondClosestDistance, object secondClosestS1, object secondClosestS2) // Changed to 'object'
{
    trialData.proximityLogs.Add(new ProximityLogEntry
    {
        timestamp = elapsedTime,
        closestIndex = closestIndex,
        closestPosition = closestPosition,
        closestAngle = closestAngle,
        closestWidth = closestWidth,
        closestDepth = closestDepth,
        closestDistance = closestDistance,
        closestSpeaker1 = (SpeakerOutput)closestS1,
        closestSpeaker2 = (SpeakerOutput)closestS2,

        secondClosestIndex = secondClosestIndex,
        secondClosestPosition = secondClosestPosition,
        secondClosestAngle = secondClosestAngle,
        secondClosestWidth = secondClosestWidth,
        secondClosestDepth = secondClosestDepth,
        secondClosestDistance = secondClosestDistance,
        secondClosestSpeaker1 = (SpeakerOutput)secondClosestS1,
        secondClosestSpeaker2 = (SpeakerOutput)secondClosestS2
    });
}

    // Fixed: New public method for GameManager to call
    public void SaveAndClearData(int condition, int trial)
    {
        Debug.Log("Saving and clearing trial data...");
        // Call the coroutine with the new parameters
        StartCoroutine(SaveFile(condition, trial));
    }

    // Fixed: Updated coroutine that handles file saving with parameters
    private IEnumerator SaveFile(int condition, int trial)
    {
        string folder = Path.Combine(Application.persistentDataPath, "Data");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        
        for(int i = 0; i < trialData.timestamps.Count; i++) {
            trialData.trialInfo.Add($"Trial {trial}");
            trialData.participantNo.Add(participantNo);
            trialData.condition.Add($"Condition {condition}");
        }

        string fileName = $"P{participantNo}_C{condition}_T{trial}.json";
        string fullPath = Path.Combine(folder, fileName);

        string jsonString = JsonConvert.SerializeObject(trialData, Formatting.Indented);
        File.WriteAllText(fullPath, jsonString);

        Debug.Log($"[SaveData] Wrote file: {fullPath}");

        trialData.ClearData();
        lastSphereEvent = null;
        yield break;
    }
}