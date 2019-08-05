using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class Client_1 : MonoBehaviour
{
    public ServiceProvider refrnce;

    // Start is called before the first frame update
    void Start()
    {
        refrnce.updateClient += ReceiveUpdates;
    }

    void ReceiveUpdates(string update)
    {
        Debug.Log(update);
        refrnce.updateClient -= ReceiveUpdates;
    }
}