using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofLayer
{
    // Start is called before the first frame update

    //Roof corner coords for simple plane to be placed
    public Vector3 _NE, _NW, _SE, _SW;
    private Vector3[] corners = new Vector3[4];
    public RoofLayer(Vector3 NE, Vector3 NW, Vector3 SE, Vector3 SW)
    {
        _NE = NE;
        _NW = NW;
        _SE = SE;
        _SW = SW;
        corners[0] = _NE;
        corners[1] = _NW;
        corners[2] = _SW;
        corners[3] = _SE;
    }

    public void visualiseLayer()
    {
        //TODO IMPLEMENT LINE DRAWING BETWEEN POINTS
        //var gameObject = new GameObject();
        //var lr = gameObject.AddComponent<LineRenderer>();

        //lr.startColor = Color.red;
        //lr.SetPositions(corners);


    }
}
