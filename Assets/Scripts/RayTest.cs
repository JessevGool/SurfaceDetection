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

    private List<RaycastHit> _hits = new List<RaycastHit>();
    private List<RoofLayer> _roofLayers = new List<RoofLayer>();
    private Vector3 _collision = Vector3.zero;
    private int _rays = 0;
    private Vector3 _rayPos;
    private Thread _t1;
    private Thread _t2;
    private bool _t1Finished = false;
    private bool _t2Finished = false;

    public bool drawHits = false;
    public bool drawLayers = false;
    public bool drawCorners = false;
    public bool startScanBool = false;
    public bool threading = false;

    [Range(0.01f, 1f)]
    public float scanResolution = 0.05f;
    [Range(0, 100)]
    public int gridSize = 100;
    public GameObject hitIndicator;
    public LayerMask rayLayer;
    public GameObject NWIND;
    public GameObject NEIND;
    public GameObject SWIND;
    public GameObject SEIND;

    private RaycastHit _hit;

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
                        _rayPos = new Vector3(x, transform.position.y, z);
                        _rays++;
                        if (Physics.Raycast(_rayPos, Vector3.down, out _hit, 10000f, rayLayer))
                        {

                            _hits.Add(_hit);


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

        if (threading)
        {
            _t1 = new Thread(createLayers);
            _t2 = new Thread(detectSlopes);
            _t1.Start();
            _t2.Start();
        }
        else
        {
            createLayers();
            detectSlopes();
        }
      
    }

    private void createLayers()
    {
        Vector3 NE = Vector3.zero, NW = Vector3.zero, SE = Vector3.zero, SW = Vector3.zero;
        List<float> uniqueHeights = new List<float>();
        List<float> zCoords = new List<float>();
        List<float> xCoords = new List<float>();
        foreach (var hit in _hits)
        {
            if (!uniqueHeights.Contains(hit.point.y))
            {
                uniqueHeights.Add(hit.point.y);

            }
        }
        foreach (float height in uniqueHeights)
        {
            foreach (var hit in _hits)
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
            _roofLayers.Add(layer);

        }
        foreach (var _layer in _roofLayers)
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
        _t1Finished = true;
    }

    private void detectSlopes()
    {

        List<float> angles = new List<float>();
        List<float> anglesX = new List<float>();
        List<RoofLayer> angledLayers = new List<RoofLayer>();
        List<float> _tempAngles = new List<float>();
        List<float> _tempAnglesX = new List<float>();
        foreach (var hit in _hits)
        {
            var angle = Vector3.Angle(gameObject.transform.forward, hit.normal) - 90;
            var angleX = Vector3.Angle(hit.transform.right, hit.normal);
            Debug.Log($"{angle} {angleX}");
            if (angle != 0f)
            {
                if (angles.Count > 0)
                {
                    foreach (float angleInList in angles)
                    {
                        if (angle > angleInList + 0.005f || angle < angleInList - 0.005f)
                        {
                            _tempAngles.Add(angle);

                        }
                    }
                }
                else
                {
                    angles.Add(angle);
                }
            }

            //TODO FIX THIS contains right angles but also wrong
            if (angleX != 90f && angle == 0f)
            {
                if (anglesX.Count > 0)
                {
                    foreach (float angleInListX in anglesX)
                    {
                        if (angleX > angleInListX + 0.005f || angleX < angleInListX - 0.005f)
                        {
                            _tempAnglesX.Add(angleX);
                        }
                    }
                }
                else
                {
                    anglesX.Add(angleX);
                }
            }
        }
        foreach (var angle in _tempAngles)
        {
            angles.Add(angle);
        }
        _tempAngles = null;
        int count = 0;
        angles = angles.Distinct().ToList();
        foreach (var angle in angles)
        {
            if (count == 4)
            {
                count = 0;
            }
            List<Vector3> angledHits = new List<Vector3>();
            float angleOffsetUp = angle + 0.005f;
            float angleOffsetDown = angle - 0.005f;
            foreach (var hit in _hits)
            {

                if (Vector3.Angle(gameObject.transform.forward, hit.normal) - 90 <= angleOffsetUp && Vector3.Angle(gameObject.transform.forward, hit.normal) - 90 >= angleOffsetDown)
                {
                    angledHits.Add(hit.point);
                }

            }
            foreach (var angledHit in angledHits)
            {
                var test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                test.name = angle.ToString();
                test.transform.position = angledHit;
                test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                if (count == 0)
                {
                    test.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (count == 1)
                {
                    test.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (count == 2)
                {
                    test.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (count == 3)
                {
                    test.GetComponent<Renderer>().material.color = Color.yellow;
                }


            }
            count++;
        }

        //TODO FIX THiS draws wrong and right angles
        foreach (var angle in _tempAnglesX)
        {
            anglesX.Add(angle);
        }
        _tempAnglesX = null;
        int count2 = 0;
        anglesX = anglesX.Distinct().ToList();
        foreach (var angleX in anglesX)
        {
            if (count2 == 4)
            {
                count2 = 0;
            }
            List<Vector3> angledHits = new List<Vector3>();
            float angleOffsetUp = angleX + 0.005f;
            float angleOffsetDown = angleX - 0.005f;
            foreach (var hit in _hits)
            {
                if (Vector3.Angle(hit.transform.right, hit.normal) <= angleOffsetUp && Vector3.Angle(hit.transform.right, hit.normal) >= angleOffsetDown)
                {
                    angledHits.Add(hit.point);
                }

            }
            foreach (var angledHit in angledHits)
            {
                var test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                test.name = angleX.ToString();
                test.transform.position = angledHit;
                test.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                if (count2 == 0)
                {
                    test.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (count2 == 1)
                {
                    test.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (count2 == 2)
                {
                    test.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (count2 == 3)
                {
                    test.GetComponent<Renderer>().material.color = Color.yellow;
                }


            }
            count2++;
        }
        _t2Finished = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_collision, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (startScanBool)
        {
            startScan();
            if (_t1Finished && _t2Finished)
            {
                Debug.Log($"AMOUNT OF RAYS: {_rays} \n" +
                            $"DETECTED HITS: {_hits.Count} \n" +
                            $"DETECTED LAYERS: {_roofLayers.Count}");
                if (drawHits)
                {
                    Instantiate(hitIndicator, _hit.point, Quaternion.LookRotation(_hit.normal));
                }
                _hits.Clear();
            }


            startScanBool = false;
        }
    }
}
