using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Linq;

public class playerparent : MonoBehaviour
{
    Vector3 playercenter;
    string playerfile;
    Rigidbody rb;
    public float speed = 20f;
    public float velVal = 0f;
    public UDPSend udpsender;
    public AudioSource[] audioSources;
    public s1 _s1;
    // public s2 _s2;
    // public s3 _s3;

    void Awake()
    {
        udpsender = GetComponent<UDPSend>();

        // null check
        if (udpsender == null)
        {
            Debug.LogError(" UDPSend component not found on the parent GameObject. Please add it.");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player: Rigidbody component not found on the parent GameObject. Please add it.");
            enabled = false;
            return;
        }

        playercenter = this.gameObject.transform.position;
        playerfile = Path.Combine(Application.persistentDataPath, "CollisionLog.csv");

        // Ensure the CSV header is written only if the file doesn't exist
        // if (!File.Exists(playerfile))
        // {
        //     File.WriteAllText(playerfile, "Timestamp,Player_X,Player_Y,Player_Z,Colliding_Child_Name,Object_Name,Object_X,Object_Y,Object_Z\n");
        // }
        // Debug.Log("Collision log file path: " + playerfile);
    }

    // Update is called once per frame
    void Update()
    {
        playercenter = this.gameObject.transform.position; // Update player's current position

        // Player movement logic
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(0, 0, 1f * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(0, 0, -1f * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-1f * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(1f * speed, 0, 0);
        }

        // if (_s1.hitObject == true)
        // {
        // playsound
        // audioSources[0].Play(); 

        // }

        velVal = rb.velocity.magnitude; // Update velocity magnitude
    }
    public void LogChildCollision(GameObject collidingChild, GameObject otherObject, Vector3 otherObjectCenter)
    {
        Vector3 currentPlayerCenter = this.gameObject.transform.position; // Get current player center for logging
        // string Timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // string logLine = string.Format(
        //     "{0},{1},{2},{3},{4},{5},{6},{7},{8}\n",
        //     Timestamp,
        //     currentPlayerCenter.x, currentPlayerCenter.y, currentPlayerCenter.z,
        //     collidingChild.name,
        //     otherObject.name,
        //     otherObjectCenter.x, otherObjectCenter.y, otherObjectCenter.z
        // );
        // File.AppendAllText(playerfile, logLine);
        Debug.Log($"Logged collision: Part '{collidingChild.name}' of belt hit '{otherObject.name}'");
    }

    //  general player unit exit/destroy events 
    void OnTriggerExit(Collider collision)
    {
        // This will trigger if the parent player capsule itself exits a trigger.
        // If you only want child-specific exits, you can remove this.
        udpsender.strMessage = "10.0," + velVal.ToString("F1");
        Debug.Log("Player exited a trigger.");
    }

    void OnDestroy()
    {
        if (udpsender != null)
        {
            udpsender.strMessage = "10.0," + velVal.ToString("F1");
            //Debug.Log("Player GameObject destroyed.");
        }
    }
}
//     void OnTriggerEnter(Collider other)
//     {
//         // Only process if this is one of the child spheres
//         if (!other.transform.IsChildOf(transform)) return;

//         string childName = other.gameObject.name;

//         int zone = GetZoneFromChildName(childName);

//         Vector3 playerPos = transform.position;
//         Vector3 forward = transform.forward;
//         Vector3 hitPoint = other.ClosestPoint(playerPos);

//         Vector3 toHit = hitPoint - playerPos;
//         toHit.y = 0f;
//         forward.y = 0f;

//         float angle = Vector3.SignedAngle(forward, toHit, Vector3.up);
//         if (angle < 0) angle += 360f;

//         string udpMessage = $"{zone}.0,0.0";
//         udpsender.strMessage = udpMessage;

//         Debug.Log($"Child '{childName}' → Zone {zone} | Angle: {angle:F1}°");
//     }

//     void OnTriggerExit(Collider other)
//     {
//         if (!other.transform.IsChildOf(transform)) return;

//         udpsender.strMessage = $"10.0,0.0";
//         Debug.Log($"Child '{other.gameObject.name}' exited.");
//     }

//     int GetZoneFromChildName(string childName)
//     {
//         // Direct match for names: "1", "2", ..., "6"
//         switch (childName)
//         {
//             case "1": return 1;
//             case "2": return 2;
//             case "3": return 3;
//             case "4": return 4;
//             case "5": return 5;
//             case "6": return 6;
//             default:
//                 Debug.LogWarning($"Unknown child part name: {childName}");
//                 return 0;
//         }
//     }
// }
