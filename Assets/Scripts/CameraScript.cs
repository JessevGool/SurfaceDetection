using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float cameraRotateSpeed = 2.0f;
    private float cameraMoveSpeed = 0.25f;

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
        if (Input.mouseScrollDelta == Vector2.down)
            Camera.main.orthographicSize += 5;
        else if (Input.mouseScrollDelta == Vector2.up)
            Camera.main.orthographicSize -= 5;

        if (Camera.main.orthographicSize < 5)
            Camera.main.orthographicSize = 5;
        else if (Camera.main.orthographicSize > 200)
            Camera.main.orthographicSize = 200;

        // rotate left/right, up/down
        //if (Input.GetMouseButton(1))
        //{
        //    //cameraHolder
        //    transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * cameraRotateSpeed, Space.Self);
        //    Camera.main.transform.Rotate(Vector3.right * Input.GetAxisRaw("Mouse Y") * cameraRotateSpeed, Space.Self);
        //}

        // Move
        if (Input.GetMouseButton(2))
        {
            //cameraHolder
            //transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * cameraMoveSpeed, 0, Input.GetAxisRaw("Mouse Y") * cameraMoveSpeed);



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
    }
}

//if (Input.GetMouseButtonDown(0))
//    Debug.Log("Pressed primary button.");

//if (Input.GetMouseButtonDown(1))
//    Debug.Log("Pressed secondary button.");

//if (Input.GetMouseButtonDown(2))
//    Debug.Log("Pressed middle click.");
