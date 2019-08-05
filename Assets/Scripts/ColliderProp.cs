using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderProp : MonoBehaviour
{
    public bool     disableRedCol = false;
    public bool     disableBlueCol = false;
    public bool     disableYellowCol = false;
    public bool     disableGreenCol = false;


    private GameObject upCollider;
    private GameObject downCollider;
    private GameObject leftCollider;
    private GameObject rightCollider;

    // Use this for initialization
    void Start () {
        upCollider = transform.GetChild(0).Find("Red").gameObject;
        downCollider = transform.GetChild(0).Find("Blue").gameObject;
        leftCollider = transform.GetChild(0).Find("Green").gameObject;
        rightCollider = transform.GetChild(0).Find("Yellow").gameObject;
        if (Application.isPlaying)
        {
            for (int i = 0; i < upCollider.transform.childCount; i++)
                Destroy(upCollider.transform.GetChild(i).gameObject);
            for (int i = 0; i < downCollider.transform.childCount; i++)
                Destroy(downCollider.transform.GetChild(i).gameObject);
            for (int i = 0; i < leftCollider.transform.childCount; i++)
                Destroy(leftCollider.transform.GetChild(i).gameObject);
            for (int i = 0; i < rightCollider.transform.childCount; i++)
                Destroy(rightCollider.transform.GetChild(i).gameObject);
            Destroy(this);

        }
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Transform par = null;
                par = upCollider.transform.GetChild(0);
                upCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, par.transform.GetChild(0).position);
                upCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, par.transform.GetChild(1).position);
                
                par = leftCollider.transform.GetChild(0);
                leftCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, par.transform.GetChild(0).position);
                leftCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, par.transform.GetChild(1).position);

                par = downCollider.transform.GetChild(0);
                downCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, par.transform.GetChild(0).position);
                downCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, par.transform.GetChild(1).position);

                
                par = rightCollider.transform.GetChild(0);
                rightCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(0, par.transform.GetChild(0).position);
                rightCollider.transform.GetChild(0).GetComponent<LineRenderer>().SetPosition(1, par.transform.GetChild(1).position);
            }
            if(disableRedCol)
                upCollider.SetActive(false);
            else
                upCollider.SetActive(true);
            if (disableBlueCol)
                downCollider.SetActive(false);
            else
                downCollider.SetActive(true);
            if (disableGreenCol)
                leftCollider.SetActive(false);
            else
                leftCollider.SetActive(true);
            if (disableYellowCol)
                rightCollider.SetActive(false);
            else
                rightCollider.SetActive(true);
       #endif
    }
}