using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @author Jesse van Gool & Maurice Brouwers
 * @version 1.0
 */
public class buttonHandler : MonoBehaviour
{
    /**
     * boolean responsible for switching between the main and the settings menu
     */
    public bool viewSettings = false;
    /**
     * parent object used for the main menu
     */
    public GameObject mainMenu;
    /**
     * parent object used for the settings menu
     */
    public GameObject settingsMenu;
    /**
     * This method triggers when a button in the GUI is pressed.
     * The method will hide or show the settings menu based on the viewSettings variable
     * @see viewSettings
     */
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
