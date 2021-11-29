using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonHandler : MonoBehaviour
{
    public bool viewSettings = false;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public void toggleSettings()
    {
        viewSettings = !viewSettings;
        if (viewSettings)
        {
            for (int i = 0; i < settingsMenu.transform.childCount; i++)
            {
                var child = settingsMenu.transform.GetChild(i).gameObject;
                if(child!= null)
                {
                    child.SetActive(true);
                }
                   
            }
            for (int i = 0; i < mainMenu.transform.childCount; i++)
            {
                var child = mainMenu.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(false);
                }

            }
        }
        else
        {
            for (int i = 0; i < settingsMenu.transform.childCount; i++)
            {
                var child = settingsMenu.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(false);
                }

            }
            for (int i = 0; i < mainMenu.transform.childCount; i++)
            {
                var child = mainMenu.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }

            }
        }
    }
}
