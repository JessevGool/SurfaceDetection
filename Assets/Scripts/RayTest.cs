using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    // Start is called before the first frame update
    private List<RaycastHit> hits = new List<RaycastHit>();
    private List<RoofLayer> roofLayers = new List<RoofLayer>();
    private Vector3 collision = Vector3.zero;
    
    private Vector3 rayPos;
    
    private bool firstScan = true;

    public bool drawHits = false;
    public GameObject debugObject;
    public GameObject hitIndicator;
    public LayerMask rayLayer;
    public GameObject NWIND;
    public GameObject NEIND;
    public GameObject SWIND;
    public GameObject SEIND;

    void Start()
    {
        startScan();
    }

    private void startScan()
    {
        RaycastHit hit;
        if (firstScan)
        {

            //X axis increment
            for (float x = transform.position.x - 10f; x < transform.position.x + 10f; x += 0.01f)
            {
                //Z axis increment
                for (float z = transform.position.z - 10f; z < transform.position.z + 10f; z += 0.01f)
                {
                    //Set Ray origin
                    rayPos = new Vector3(x, transform.position.y, z);


                    if (Physics.Raycast(rayPos, Vector3.down, out hit, 10000f, rayLayer))
                    {
                        //Debug.Log($"{hit.point}");
                        hits.Add(hit);
                        //Create sphere to show hit
                        if(drawHits)
                        {
                            Instantiate(hitIndicator, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                        
                    }
                    debugObject.transform.position = new Vector3(x, transform.position.y, z);

                }
            }
            //was used inside update
            firstScan = !firstScan;
            createLayers(hits);
        }
    }

    private void createLayers(List<RaycastHit> roofHits)
    {
        Vector3 NE = Vector3.zero, NW = Vector3.zero, SE = Vector3.zero, SW = Vector3.zero;
        List<float> uniqueHeights = new List<float>();
        List<float> zCoords = new List<float>();
        List<float> xCoords = new List<float>();
        foreach (var hit in roofHits)
        {
            if (!uniqueHeights.Contains(hit.point.y))
            {
                uniqueHeights.Add(hit.point.y);
                
            }
        }
        foreach (float height in uniqueHeights)
        {
            foreach (var hit in roofHits)
            {
                if (hit.point.y == height)
                {
                    zCoords.Add(hit.point.z);
                    xCoords.Add(hit.point.x);
                }

            }
            //Set corner coords
            NE = new Vector3(xCoords.Min(), height, zCoords.Min());
            NW = new Vector3(xCoords.Max(), height, zCoords.Min());
            SE = new Vector3(xCoords.Min(), height, zCoords.Max());
            SW = new Vector3(xCoords.Max(), height, zCoords.Max());

            //Clear lists for next layer
            zCoords.Clear();
            xCoords.Clear();
            RoofLayer layer = new RoofLayer(NE, NW, SE, SW);
            roofLayers.Add(layer);
        }
        foreach (var _layer in roofLayers)
        {
            Debug.Log($"NE: {_layer._NE}  NW: {_layer._NW}  SE: {_layer._SE}  SW: {_layer._SW}");

            //Indicate layers by drawing Spheres
            if (!drawHits)
            {
                Instantiate(NEIND, _layer._NE, Quaternion.LookRotation(_layer._NE));
                Instantiate(NWIND, _layer._NW, Quaternion.LookRotation(_layer._NW));
                Instantiate(SEIND, _layer._SE, Quaternion.LookRotation(_layer._SE));
                Instantiate(SWIND, _layer._SW, Quaternion.LookRotation(_layer._SW));
            }
           

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
