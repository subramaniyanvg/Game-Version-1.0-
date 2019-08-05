using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorScalarMultiplication : MonoBehaviour
{

    public GameObject vector;
    public float   scaleAmt;
    
    public Color resultantCol;

    private void OnDrawGizmos()
    {
        Gizmos.color = resultantCol;
        Gizmos.DrawLine(Vector3.zero, vector.transform.position * scaleAmt);
    }
}