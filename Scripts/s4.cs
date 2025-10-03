using UnityEngine;

public class s4 : MonoBehaviour
{
    private playerparent parentPlayerScript; // Reference to the parent's script
    public bool hitObject = false; 

    void Awake()
    {
        // Get the playerscript component from the parent GameObject
        parentPlayerScript = GetComponentInParent<playerparent>();

        if (parentPlayerScript == null)
        {
            Debug.LogError($"s4 on '{gameObject.name}': Parent 'playerscript' not found. This script requires a 'playerscript' on its parent.");
            enabled = false; // Disable this script if parent script is missing
            return;
        }

        // Ensure this GameObject has a Collider and it's set to Is Trigger
        // (You should have already done this in the Unity Editor, but this is a good check)
        // Collider col = GetComponent<Collider>();
        // if (col == null)
        // {
        //     Debug.LogError($"s4 on '{gameObject.name}': No Collider found. Please add a Collider component.");
        //     enabled = false;
        //     return;
        // }
        // if (!col.isTrigger)
        // {
        //     Debug.LogWarning($"s4 on '{gameObject.name}': Collider is not set to 'Is Trigger'. Setting it now.");
        //     col.isTrigger = true;
        // }

        // // Ensure this GameObject has a Rigidbody
        // Rigidbody rb = GetComponent<Rigidbody>();
        // if (rb == null)
        // {
        //     Debug.LogError($"s4 on '{gameObject.name}': No Rigidbody found. Please add a Rigidbody component.");
        //     enabled = false;
        //     return;
        // }
       
        // // rb.isKinematic = true; 
    }

    void OnTriggerEnter(Collider other)
    {
          
            // Get the parent's current velocity magnitude
            float parentVelVal = parentPlayerScript.velVal;

            
            string udpMessage = "4.0," + parentVelVal.ToString("F1");

            // Send the UDP message via the parent's UDPSend component
            parentPlayerScript.udpsender.strMessage = udpMessage;

            hitObject = true; 

            Debug.Log($"4 collided with trigger '{other.gameObject.name}'");

            // Logging the collision
           // parentPlayerScript.LogChildCollision(this.gameObject, other.gameObject, other.gameObject.transform.position);
        
    }

   
    void OnTriggerExit(Collider other)
    {
        
        
            // Send the UDP message when exiting, including velocity
            parentPlayerScript.udpsender.strMessage = "10.0," + parentPlayerScript.velVal.ToString("F1");
            Debug.Log($"4 exited trigger '{other.gameObject.name}'.");
        
    }
}
