using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector : MonoBehaviour
{
    public GameObject startPnt;
    public GameObject endPnt;
    public Color      vectorCol;

    private void OnDrawGizmos()
    {
        Vector3 one = new Vector3(1, 0, 0);

        one = (one * 2);
        Debug.Log(endPnt.transform.position - startPnt.transform.position);
        Debug.Log((endPnt.transform.position - startPnt.transform.position).magnitude);
        Gizmos.color = vectorCol;
        Gizmos.DrawLine(startPnt.transform.position, endPnt.transform.position);
    }
}
