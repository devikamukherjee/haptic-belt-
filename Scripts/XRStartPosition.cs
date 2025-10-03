using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRStartPosition : MonoBehaviour
{
    public Transform startPoint;  

    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }
        else
        {
            Debug.LogWarning("Start Point not assigned in XRStartPosition script.");
        }
    }
}
