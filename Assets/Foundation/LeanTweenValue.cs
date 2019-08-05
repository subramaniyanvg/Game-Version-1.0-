using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class LeanTweenValue : MonoBehaviour
{
    LTDescr desc = null;
    public LeanTweenType ease = LeanTweenType.linear;
    public float compSecs;
    
    // Start is called before the first frame update
    void Start()
    {
        desc = LeanTween.value(gameObject, 0, 1, compSecs).setOnUpdate(UpdateValue).setOnComplete(Complete).setEase(ease);
        LeanTween.value(gameObject, 0, 1, compSecs).setOnUpdate(UpdateValue1).setOnComplete(Complete1).setEase(ease);
    }

    void UpdateValue(float val)
    {
        Debug.Log("Update Value : " + val);
    }

    void Complete()
    {
        Debug.Log("Complete");
    }

    void UpdateValue1(float val)
    {
        Debug.Log("Update Value 1: " + val);
    }

    void Complete1()
    {
        Debug.Log("Complete 1");
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