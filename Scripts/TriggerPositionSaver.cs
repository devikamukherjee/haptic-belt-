using UnityEngine;
using System.Collections.Generic; // Required for List
using System.IO;                  // Required for File operations

public class TriggerPositionSaver : MonoBehaviour
{
    // Public array to store the positions, visible in the Inspector (optional, but good for debugging)
    public Vector3[] triggerPositions;

    // The name of the CSV file
    public string fileName = "TriggerPositions.csv";

    // The folder to save the CSV file. Application.dataPath is your project's Assets folder.
    // Application.persistentDataPath is generally better for user data as it's outside the project folder.
    // For editor-time saving, Application.dataPath is convenient.
    public string saveFolder; 

    void Awake()
    {
        // Set the save folder. For editor scripts or development, Application.dataPath is useful.
        // For builds, Application.persistentDataPath is recommended.
        saveFolder = Application.dataPath + "/SavedPositions/"; 
        
        // Ensure the directory exists
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
            Debug.Log("Created directory: " + saveFolder);
        }
    }

    void Start()
    {
        SaveTriggerPositionsToCSV();
    }

    void SaveTriggerPositionsToCSV()
    {
        // Find all GameObjects with the tag "trigger"
        GameObject[] triggerObjects = GameObject.FindGameObjectsWithTag("trigger");

        // Create a temporary list to hold the positions
        List<Vector3> tempPositions = new List<Vector3>();

        // Iterate through each found trigger object and add its position to the list
        foreach (GameObject triggerObj in triggerObjects)
        {
            tempPositions.Add(triggerObj.transform.position);
        }

        // Convert the list to a fixed-size array (optional, but keeps the public variable updated)
        triggerPositions = tempPositions.ToArray();

        // Construct the full file path
        string fullPath = Path.Combine(saveFolder, fileName);

        // Create a list of strings for the CSV lines
        List<string> csvLines = new List<string>();

        // Add the header row
        csvLines.Add("X,Y,Z");

        // Add each position as a new line
        foreach (Vector3 pos in triggerPositions)
        {
            // Format the coordinates. Using "F6" for 6 decimal places for precision.
            // Using InvariantCulture to ensure dot as decimal separator regardless of system locale.
            csvLines.Add(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F6},{1:F6},{2:F6}", pos.x, pos.y, pos.z));
        }

        try
        {
            // Write all lines to the file
            File.WriteAllLines(fullPath, csvLines);
            Debug.Log("Successfully saved trigger positions to CSV: " + fullPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to write CSV file: " + e.Message);
        }

        // Optional: Print the saved positions to the Console for verification
        Debug.Log("Saved " + triggerPositions.Length + " trigger positions:");
        for (int i = 0; i < triggerPositions.Length; i++)
        {
            Debug.Log("Trigger " + i + " position: " + triggerPositions[i]);
        }
    }
}