using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ApplicationMenu : MonoBehaviour
{

    public GameObject objectModel;

    Vector3 scaleVector;
    float scale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        scaleVector = new Vector3(scale, scale, scale);
        //string objectName = Path.GetFileNameWithoutExtension(MainMenu.objectPath);
        //Debug.Log(objectName);
        //objectModel = GameObject.Find("/" + objectName);

    }

    public void InitializeGameObject(GameObject gameObject)//string objectName)
    {
        objectModel = gameObject;//GameObject.Find("/" + objectName);
        if (objectModel == null)
        {
            Debug.Log("object is null");
        }
    }

    public void ScaleButton()
    {
        scale += 0.5f;
 


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
