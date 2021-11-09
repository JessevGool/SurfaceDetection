using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationMenu : MonoBehaviour
{

    public GameObject objectModel;
    
    private RayTest rayTest;

    Vector3 scaleVector;
    float scale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        scaleVector = new Vector3(scale, scale, scale);
        rayTest = FindObjectOfType<RayTest>();

    }

    public void InitializeGameObject(GameObject gameObject)
    {
        objectModel = gameObject;
        if (objectModel == null)
        {
            Debug.Log("object is null");
        }
        else
        {
            //objectModel.AddComponent<MeshCollider>();
            objectModel.transform.Rotate(-90, 0, 0);
        }
    }

    public void ScaleButton()
    {
        //scale += 0.5f;

        rayTest.startScanBool = true;

    }



    // Update is called once per frame
    void Update()
    {
        if (objectModel != null)
        {
           

            objectModel.transform.localScale = scaleVector;
        }

    }
}
