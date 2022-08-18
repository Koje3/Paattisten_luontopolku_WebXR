using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDeviceOrientation : MonoBehaviour
{


    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            GetComponent<Camera>().fieldOfView = 40f;
        }
        else
        {
            GetComponent<Camera>().fieldOfView = 60f;
        }
    }
}
