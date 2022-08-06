using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGyro : MonoBehaviour
{
    public Camera arCamera;
    public GameObject startPosition;
    public GameObject cameraPositions;

    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private Quaternion rot;

    private float lerpDuration = 0.5f;
    private float lerpRotationDuration = 0.5f;
    private bool changeCameraPosition;

    private GameObject currentCameraPosition;
    private GameObject previousCameraPosition;
    private Vector3 cameraDestination;

    private GameObject hitObject;

    //FOR TOUCH ROTATION
    private Vector3 firstpoint;
    private Vector3 secondpoint;
    private float xAngle;
    private float yAngle;
    private float xAngTemp;
    private float yAngTemp;

    void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();

        currentCameraPosition = startPosition;
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

        if (Input.touchCount > 0 && !changeCameraPosition)
        {
            Touch touch1 = Input.GetTouch(0);

            if (Input.touchCount < 2)
            {
                if (touch1.phase == TouchPhase.Began)
                {
                    RaycastChangePosition(touch1);

                    //TOUCH ROTATION
                    firstpoint = touch1.position;
                    xAngTemp = xAngle;
                    yAngTemp = yAngle;
                }

                if (touch1.phase == TouchPhase.Moved)
                {
                    //TOUCH ROTATION
                    secondpoint = touch1.position;
                    //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                    xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180f / Screen.width;
                    yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90f / Screen.height;
                    //Rotate camera
                    cameraContainer.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
                }
            }

        }


    }

    public IEnumerator ChangePositionSmooth(Vector3 cameraDestination)
    {
        float timeElapsed = 0f;
        changeCameraPosition = true;
        Vector3 startPosition = cameraContainer.transform.position;

        while (timeElapsed < lerpDuration)
        {
            cameraContainer.transform.position = Vector3.Lerp(startPosition, cameraDestination, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        changeCameraPosition = false;
        cameraContainer.transform.position = cameraDestination;
    }

    public void RaycastChangePosition(Touch touch)
    {
        Ray ray = arCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.transform.gameObject;

            Debug.Log(hitObject);

            if (hitObject.tag == "StellarObject")
            {
                previousCameraPosition = currentCameraPosition;
                currentCameraPosition = cameraPositions.transform.Find(hitObject.name).gameObject;

                cameraDestination = currentCameraPosition.transform.position;
                StartCoroutine(ChangePositionSmooth(cameraDestination));

                //previousCameraPosition.SetActive(true);
                //currentCameraPosition.SetActive(false);

            }
        }
    }

    public void RotateCameraTouch()
    {
        
    }

}
