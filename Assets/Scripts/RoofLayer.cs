using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofLayer
{
    // Start is called before the first frame update

    //Roof corner coords for simple plane to be placed
    public Vector3 _NE, _NW, _SE, _SW;
    public RoofLayer(Vector3 NE, Vector3 NW, Vector3 SE, Vector3 SW)
    {
        _NE = NE;
        _NW = NW;
        _SE = SE;
        _SW = SW;
    }
}
