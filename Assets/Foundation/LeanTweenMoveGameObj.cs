using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class LeanTweenMoveGameObj : MonoBehaviour
{
    LTDescr desc;
    public  Transform obj;
    public Vector3 pos;
    public LeanTweenType ease = LeanTweenType.linear;
    public float compSecs;

    // Start is called before the first frame update
    void Start()
    {
        if(obj != null)
            desc = LeanTween.move(gameObject, obj.transform, compSecs).setOnComplete(Complete).setEase(ease);
        else
            desc = LeanTween.move(gameObject, pos, compSecs).setOnComplete(Complete).setEase(ease);
    }

    void Complete()
    {
        Debug.Log("Complete");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            LeanTween.cancel(gameObject,false);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            LeanTween.cancel(gameObject, desc.id,false);
    }
}