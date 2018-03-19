using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleVector {

    public Vector3 start;
    public Vector3 end;

    public DoubleVector() {
        start = new Vector3(0, 0, 0);
        end = new Vector3(0, 0, 0);
    }

    public DoubleVector(Vector3 s, Vector3 e)
    {
        start =s;
        end = e;
    }
}
