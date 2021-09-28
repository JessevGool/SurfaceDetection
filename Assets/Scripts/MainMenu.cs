using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadObjectModel()
    {





        SceneManager.LoadScene(1);

    }



    public void QuitApplication()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
