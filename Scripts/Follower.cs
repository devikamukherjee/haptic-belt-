using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform myXRigPos; 
    public Transform myXRig;
    public Vector3 offset; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myXRigPos.position + offset; 
        transform.localRotation = Quaternion.Euler(0f,myXRig.localEulerAngles.y,0f);
    }
}
