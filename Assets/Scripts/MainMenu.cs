using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string objectPath = "";

    public void LoadObjectModel()
    {

        Debug.Log("loading scene 1");
        SceneManager.LoadScene(1);

    }










    public void SelectObjModelFromFile()
    {
        //System.Diagnostics.Process p = new System.Diagnostics.Process();
        //p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
        //p.Start();


        objectPath = EditorUtility.OpenFilePanel("Show all .obj files", "", "obj");
        Debug.Log(objectPath);
    }

    public void QuitApplication()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
