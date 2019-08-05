using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngBetweenTwoVectors : MonoBehaviour
{
    public GameObject    vectorA;
    public GameObject    vectorB;

    private void Update()
    {
        Debug.Log(Vector3.Angle(vectorA.transform.right, vectorB.transform.right));
    }
}