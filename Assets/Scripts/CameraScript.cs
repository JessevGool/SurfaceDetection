using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float cameraRotateSpeed = 2.0f;
    private float cameraMoveSpeed = 0.5f;
    private float scrollSpeed = 10.0f;

    private float yaw = 0.0f;
    private float pitch = -200.0f;

    private bool mouseButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        if (Input.GetMouseButton(2))
        {
            //cameraHolder

            Vector3 projectVector = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
            transform.Translate(projectVector * Input.GetAxisRaw("Mouse Y") * -cameraMoveSpeed, Space.World);
            transform.Translate(Camera.main.transform.right * Input.GetAxisRaw("Mouse X") * -cameraMoveSpeed, Space.World);
        }



        if (Input.GetMouseButtonDown(1))
        {
            mouseButtonDown = true;
        }

        if (mouseButtonDown)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch += speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, -178.749f);
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseButtonDown = false;
        }



        //scroll
        Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
    }
}

