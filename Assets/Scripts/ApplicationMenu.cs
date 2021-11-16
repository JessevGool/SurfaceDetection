using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationMenu : MonoBehaviour
{

    public GameObject objectModel;
    
    private RayTest rayTest;

    Vector3 scaleVector;
    float scale = 1.0f;





    //addSolarPanels
    private List<Vector2> SolarPanelSizes;


    public InputField SolarLengthInputField;
    public InputField SolarWidthInputField;

    private string newSolarLength;
    private string newSolarWidth;

    public GameObject addSolarPanel;

    // Start is called before the first frame update
    void Start()
    {
        SolarPanelSizes = new List<Vector2>();
        scaleVector = new Vector3(scale, scale, scale);
        rayTest = FindObjectOfType<RayTest>();
        addSolarPanel.SetActive(false);
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
            //only tested for 1 pix4d object
            objectModel.transform.Rotate(-90, 0, 0);
        }
    }

    #region Main

    public void StartScanButton()//start scan button
    {
        //scale += 0.5f;

        rayTest.startScanBool = true;

    }

    public void AddSolarPanelButton()
    {
        if (addSolarPanel != null)
        {
            addSolarPanel.SetActive(true);
        }

    }


    public void LoadSolarPanelPlacement()
    {
        int offset = 0;
        foreach (Vector2 item in SolarPanelSizes)
        {
            GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Debug.Log("item.x: "+item.x.ToString());
            panel.transform.localScale = new Vector3(item.x, 0.2f, item.y);
            panel.transform.localPosition = new Vector3(0, offset, 0);
            
            offset += 1;
        }
        

    }

    #endregion Main





    #region AddSolarSize
    public void CancelAddSolarPanelButton()
    {
        EndOfSolarPanelUI();
    }


    public void AcceptAddSolarPanelButton()
    {
        float tempWidth = 0.0f;
        float tempLength = 0.0f;
        float.TryParse(newSolarWidth, out tempWidth);
        float.TryParse(newSolarLength, out tempLength);
        Debug.Log("temp length: "+ tempLength);
        Vector2 newSolarpanel = new Vector2(tempLength, tempWidth);

        SolarPanelSizes.Add(newSolarpanel);

        

       


        EndOfSolarPanelUI();
    }




    private void EndOfSolarPanelUI()
    {
        //forget input strings
        newSolarLength = "";
        newSolarWidth = "";

        //hide panel
        if (addSolarPanel != null)
        {
            addSolarPanel.SetActive(false);
        }
    }


    //used for updating the current input
    public void EditLengthInput()//string length)
    {
        newSolarLength = SolarLengthInputField.text;//length;
        Debug.Log(newSolarLength);
    }
    public void EditWidthInput()//string width)
    {
        newSolarWidth = SolarWidthInputField.text;//width;
        Debug.Log("width" + newSolarWidth);
    }
    #endregion AddSolarSize

    // Update is called once per frame
    void Update()
    {
        //if (objectModel != null)
        //{
           

        //    objectModel.transform.localScale = scaleVector;
        //}

    }
}
