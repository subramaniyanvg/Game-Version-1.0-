using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInfo : MonoBehaviour
{
    public string ps4ControllerName = "Wireless Controller";
    public string xBoxOneControllerName = "Controller (Xbox One For Windows)";

    public static ControllerInfo instance;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsConnectedControllerPs4()
    {
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]) && string.Equals(temp[i], ps4ControllerName))
                    return true;
            }
        }
        return false;
    }

    public bool IsConnectedControllerXboxOne()
    {
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]) && string.Equals(temp[i], xBoxOneControllerName))
                    return true;
            }
        }
        return false;
    }

    public bool IsControllerConnected()
    {
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                    return true;
            }
        }
        return false;
    }
}