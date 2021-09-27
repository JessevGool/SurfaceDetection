using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{
    // Start is called before the first frame update
    private List<RaycastHit> hits = new List<RaycastHit>();
    private Vector3 collision = Vector3.zero;
    public GameObject debugObject;
    public GameObject hitIndicator;
    private Vector3 rayPos;
    public LayerMask rayLayer;
    private bool firstScan = true;
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
            for (float x = transform.position.x - 10f; x < transform.position.x + 10f; x+= 0.01f)
            {
                //Z axis increment
                for (float z = transform.position.z - 10f; z < transform.position.z + 10f; z+= 0.01f)
                {
                    //Set Ray origin
                    rayPos = new Vector3(x, transform.position.y, z);
                    

                    if (Physics.Raycast(rayPos, Vector3.down, out hit, 100f,rayLayer))
                    {
                        Debug.Log($"{hit.point}");
                        hits.Add(hit);
                        //Create sphere to show hit
                        Instantiate(hitIndicator, hit.point,Quaternion.LookRotation(hit.normal));
                    }
                    debugObject.transform.position = new Vector3(x, transform.position.y, z);

                }
            }
            //was used inside update
            firstScan = !firstScan;
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
