using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGyro : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;

    void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
 
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
    }


    void Update()
    {
        if (gyroEnabled)
        {
            // transform.localRotation = gyro.attitude * rot;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, gyro.attitude * rot, 0.1f);
        }
    }
}
