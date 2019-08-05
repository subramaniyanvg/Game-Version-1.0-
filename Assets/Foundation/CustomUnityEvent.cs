using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<int>
{

}

public class CustomUnityEvent : MonoBehaviour
{
    private MyEvent evt;

    // Start is called before the first frame update
    void Start()
    {
        evt = new MyEvent();
        evt.AddListener(PrintNumber);
        evt.RemoveListener(PrintNumber);
    }

    public void PrintNumberFromScript()
    {
        evt.Invoke(5);
    }

    public void PrintNumber(int number)
    {
        Debug.Log(number);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}