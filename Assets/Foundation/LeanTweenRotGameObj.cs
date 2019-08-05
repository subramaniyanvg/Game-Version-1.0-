using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class LeanTweenRotGameObj : MonoBehaviour
{
    LTDescr desc;
    public GameObject    rotAxis;
    public float         rotAmt;
    public LeanTweenType ease = LeanTweenType.linear;
    public float         compSecs;

    // Start is called before the first frame update
    void Start()
    {
        desc = LeanTween.rotateAround(gameObject, rotAxis.transform.forward, rotAmt,compSecs).setOnComplete(Complete).setEase(ease);
    }

    void Complete()
    {
        Debug.Log("Complete");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            LeanTween.cancel(gameObject, false);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            LeanTween.cancel(gameObject, desc.id, false);
    }
}