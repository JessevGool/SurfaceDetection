using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using System.Threading;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;

public class RayTest : MonoBehaviour
{

    private List<RaycastHit> hits = new List<RaycastHit>();
    private List<RoofLayer> roofLayers = new List<RoofLayer>();
    private Vector3 collision = Vector3.zero;
    private int rays = 0;
    private Vector3 rayPos;

    public bool drawHits = false;
    public bool drawLayers = false;
    public bool drawCorners = false;
    public bool startScanBool = false;

    [Range(0.01f,1f)]
    public float scanResolution = 0.05f;
    [Range(0,100)]
    public int gridSize = 100;
    public GameObject hitIndicator;
    public LayerMask rayLayer;
    public GameObject NWIND;
    public GameObject NEIND;
    public GameObject SWIND;
    public GameObject SEIND;

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void startScan()
    {

        float maxBoundsX = gameObject.GetComponent<Renderer>().bounds.max.x;
        float maxBoundsZ = gameObject.GetComponent<Renderer>().bounds.max.z;
        float minBoundsX = gameObject.GetComponent<Renderer>().bounds.min.x;
        float minBoundsZ = gameObject.GetComponent<Renderer>().bounds.min.z;
        float _minBoundsX = minBoundsX < 0 ? minBoundsX * -1 : minBoundsX;
        float _minBoundsZ = minBoundsZ < 0 ? minBoundsZ * -1 : minBoundsZ;
        float totalX = _minBoundsX + maxBoundsX;
        float totalZ = _minBoundsZ + maxBoundsZ;
        float incrementZ = totalZ / gridSize;
        float incrementX = totalX / gridSize;

        //Offset at the end is not precise, without it buffer is not sufficient
        float requiredBuffer = (((maxBoundsX / scanResolution) * (maxBoundsZ / scanResolution) * 4f)) * 1.01f;
        
        Debug.Log("starting scan");
        for (int i = 0; i < gridSize; i++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //var results = new NativeArray<RaycastHit>((int)10000000, Allocator.TempJob);
                //var commands = new NativeArray<RaycastCommand>((int)10000000, Allocator.TempJob);
                float startPosX = minBoundsX + incrementX * i;
                float startPosZ = minBoundsZ + incrementZ * y;
                for (float x = startPosX; x < startPosX + incrementX; x += scanResolution)
                {
                    for (float z = startPosZ; z < startPosZ + incrementZ; z += scanResolution)
                    {
                        //Set Ray origin
                        //rayPos = new Vector3(x, transform.position.y, z);
                        //commands[rays] = new RaycastCommand(rayPos, Vector3.down, 1000f, rayLayer);
                        rayPos = new Vector3(x, transform.position.y, z);
                        rays++;
                        if (Physics.Raycast(rayPos, Vector3.down, out hit, 10000f, rayLayer))
                        {
                            
                            hits.Add(hit);
                        }
                        
                    }

                }
                //JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));
                //handle.Complete();
                //foreach (var _hit in results)
                //{
                //    if (_hit.collider != null)
                //    {
                //        hits.Add(_hit);
                //    }
                //}
                //results.Dispose();
                //commands.Dispose();
            }
            

        }
        createLayers(hits);
        detectSlopes(hits);
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

            //Indicate layers by drawing Spheres
            if (!drawHits)
            {
                if (drawCorners)
                {
                    Instantiate(NEIND, _layer._NE, Quaternion.LookRotation(_layer._NE));
                    Instantiate(NWIND, _layer._NW, Quaternion.LookRotation(_layer._NW));
                    Instantiate(SEIND, _layer._SE, Quaternion.LookRotation(_layer._SE));
                    Instantiate(SWIND, _layer._SW, Quaternion.LookRotation(_layer._SW));
                }
               

                //NOT YET IMPLEMENTED
                if (drawLayers)
                {
                    _layer.visualiseLayer();
                }
            }


        }
    }

    private void detectSlopes(List<RaycastHit> roofHits)
    {
        List<float> angles = new List<float>();
        List<float> anglesX = new List<float>();
        List<RoofLayer> angledLayers = new List<RoofLayer>();
        foreach (var hit in roofHits)
        {
            var angle = Vector3.Angle(gameObject.transform.forward, hit.normal) - 90;
            var angleX = Vector3.Angle(hit.transform.right, hit.normal);
            //angleX = Mathf.Abs(angleX - 90);

            if (angle != 0f)
            {
                if (!angles.Contains(angle))
                {
                    angles.Add(angle);
                }
            }

            //TODO FIX THIS contains right angles but also wrong
            if (angleX != 90f && angle == 0f)
            {
                if (!anglesX.Contains(angleX))
                { 
                    anglesX.Add(angleX);
                }
            }
        }
        int count = 0;
        foreach (var angle in angles)
        {
            if (count == 3)
            {
                count = 0;
            }
            List<Vector3> angledHits = new List<Vector3>();
            foreach (var hit in roofHits)
            {
                if (Vector3.Angle(gameObject.transform.forward, hit.normal) - 90 == angle)
                {
                    angledHits.Add(hit.point);
                }

            }
            foreach (var angledHit in angledHits)
            {
                var test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                test.name = angle.ToString();
                test.transform.position = angledHit;
                test.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                if (count == 0)
                {
                    test.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (count == 1)
                {
                    test.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    test.GetComponent<Renderer>().material.color = Color.blue;
                }


            }
            count++;
        }

        //TODO FIX THiS draws wrong and right angles
        int count2 = 0;
        foreach (var angleX in anglesX)
        {
            if (count2 == 3)
            {
                count2 = 0;
            }
            List<Vector3> angledHits = new List<Vector3>();
            foreach (var hit in roofHits)
            {
                if (Vector3.Angle(hit.transform.right, hit.normal) == angleX)
                {
                    angledHits.Add(hit.point);
                }

            }
            foreach (var angledHit in angledHits)
            {
                var test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                test.name = angleX.ToString();
                test.transform.position = angledHit;
                test.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                if (count2 == 0)
                {
                    test.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (count2 == 1)
                {
                    test.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    test.GetComponent<Renderer>().material.color = Color.blue;
                }


            }
            count2++;
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
        if (startScanBool)
        {
            startScan();
            Debug.Log($"AMOUNT OF RAYS: {rays} \n" +
            $"DETECTED HITS: {hits.Count} \n" +
            $"DETECTED LAYERS: {roofLayers.Count}");
            hits.Clear();
            startScanBool = false;
        }
    }
}
