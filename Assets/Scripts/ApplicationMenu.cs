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





    //addSolarPanels
    private List<Vector2> SolarPanelSizes;

    private string newSolarLength;
    private string newSolarWidth;

    public GameObject addSolarPanel;

    // Start is called before the first frame update
    void Start()
    {
        SolarPanelSizes = new List<Vector2>();
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


    }

    #endregion Main





    #region AddSolarSize
    public void CancelAddSolarPanelButton()
    {
        EndOfSolarPanelUI();
    }


    public void AcceptAddSolarPanelButton()
    {
        Vector2 newSolarpanel = new Vector2(1, 1);

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
    public void EditLengthInput(string length)
    {
        newSolarLength = length;
        Debug.Log(newSolarLength.ToString());
    }
    public void EditWidthInput(string width)
    {
        newSolarWidth = width;
        Debug.Log("width" + newSolarWidth.ToString());
    }
    #endregion AddSolarSize

    // Update is called once per frame
    void Update()
    {
        if (objectModel != null)
        {
           

            objectModel.transform.localScale = scaleVector;
        }

    }
}
