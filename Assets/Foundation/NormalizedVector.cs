using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedVector : MonoBehaviour
{
    public GameObject vector;
    public Color      vectorCol;

    public Color      normVectorCol;

    public Vector3     redVector;
    public Vector3     greenVector;
    public float      vecLength;
    public float      normVecLength;



    private void OnDrawGizmos()
    {

        Gizmos.color = vectorCol;
        Gizmos.DrawLine(Vector3.zero, vector.transform.position);

        Gizmos.color = normVectorCol;
        Gizmos.DrawLine(Vector3.zero, (vector.transform.position).normalized);

        vecLength = (vector.transform.position).magnitude;
        normVecLength = ((vector.transform.position).normalized).magnitude;

        redVector = vector.transform.position;
        greenVector = (vector.transform.position).normalized;
    }
}
