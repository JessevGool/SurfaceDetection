using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
/**
 * @author Jesse van Gool & Maurice Brouwers
 * @version 1.0
 */
public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private TMPro.TextMeshProUGUI textSliderValue;
    /**
     * Standard resolution of the scan
     * Increment of 0.05
     */
    public float resolution = 0.05f;
    /**
     * Standard gridsize of the scan on one of the axes
     * Standard is 80
     */
    public float gridSize = 80;
    private RayTest rayTest;


    void Start()
    {
        rayTest = FindObjectOfType<RayTest>();
        textSliderValue = GetComponent<TMPro.TextMeshProUGUI>();
    }
    /**
     * Update method that gets called once per frame. Is used to update settings in the rayTest class and the UI
     * 
     * @see RayTest::scanResolution
     * @see RayTest::gridSize
     */
    private void Update()
    {
        if(sliderUI.name.Contains("Scan"))
        {
            float resolution = sliderUI.value;
            resolution = (resolution - resolution % 0.01f);
            string sliderMessage = "Resolution = " + resolution;
            textSliderValue.text = sliderMessage;
            rayTest.scanResolution = resolution;
        }
        else if (sliderUI.name.Contains("Grid"))
        {
            float gridSize = sliderUI.value;
            string sliderMessage = "Grid = " + gridSize;
            textSliderValue.text = sliderMessage;
            rayTest.gridSize = gridSize;
        }
        
    }
}