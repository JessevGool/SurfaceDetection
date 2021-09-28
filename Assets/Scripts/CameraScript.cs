using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool mouseButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouseButtonDown = true;
        }

        if (mouseButtonDown)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseButtonDown = false;
        }
    }
}

//if (Input.GetMouseButtonDown(0))
//    Debug.Log("Pressed primary button.");

//if (Input.GetMouseButtonDown(1))
//    Debug.Log("Pressed secondary button.");

//if (Input.GetMouseButtonDown(2))
//    Debug.Log("Pressed middle click.");
