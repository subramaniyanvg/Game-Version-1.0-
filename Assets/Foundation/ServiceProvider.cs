using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class ServiceProvider : MonoBehaviour
{
    public delegate void     SendUpdate(string update);
    public event SendUpdate        updateClient;

    void DontUpdateClients()
    {
        updateClient = null;
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) && updateClient != null)
            updateClient.Invoke("50% Offer On Our Products");
    }
}
