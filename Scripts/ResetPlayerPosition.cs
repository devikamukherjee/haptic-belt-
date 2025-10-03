using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPosition : MonoBehaviour
{

    public Transform player; 

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Fire1"))
        {
            player.transform.position = transform.position;
        }
        
    }
}
