using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private TMPro.TextMeshProUGUI textSliderValue;
    public float resolution = 0.05f;
    public float gridSize = 80;
    private RayTest rayTest;


    void Start()
    {
        rayTest = FindObjectOfType<RayTest>();
        textSliderValue = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        if(sliderUI.name.Contains("Scan"))
        {
            float resolution = sliderUI.value;
            resolution = (resolution - resolution % 0.05f);
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