using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorAdd : MonoBehaviour
{
    public Vector3    vectorA;
    public GameObject vectorAPos;
    public Color   vectorACol;
    public Vector3 vectorB;
    public Color   vectorBCol;

    public Color   resultantVectorCol;

    private void OnDrawGizmos()
    {
        Gizmos.color = vectorACol;
        Gizmos.DrawLine(vectorAPos.transform.position, vectorAPos.transform.position + vectorA);

        Gizmos.color = vectorBCol;
        Gizmos.DrawLine(vectorAPos.transform.position + vectorA, (vectorAPos.transform.position + vectorA) + vectorB);

        Gizmos.color = resultantVectorCol;
        Gizmos.DrawLine(vectorAPos.transform.position, vectorAPos.transform.position + (vectorA + vectorB));
    }
}