using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveRight;
    public KeyCode moveLeft;
    public KeyCode moveForward;
    public KeyCode moveBack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (
                Input.GetKeyUp(moveUp) || 
                Input.GetKeyUp(moveDown) || 
                Input.GetKeyUp(moveRight) || 
                Input.GetKeyUp(moveLeft) ||
                Input.GetKeyUp(moveForward) ||
                Input.GetKeyUp(moveBack)
            )
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(moveUp))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
        }

        if (Input.GetKeyDown(moveDown))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
        }

        if (Input.GetKeyDown(moveRight))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(6, 0, 0);
        }

        if (Input.GetKeyDown(moveLeft))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-6, 0, 0);
        }

        if (Input.GetKeyDown(moveForward))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, -6, 0);
        }

        if (Input.GetKeyDown(moveBack))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 6, 0);
        }
    }
}
