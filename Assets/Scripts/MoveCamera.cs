using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Camera arCamera;
    public GameObject startPosition;
    public GameObject wakeUpPosition;
    public GameObject cameraPositions;



    private GameObject cameraContainer;
    private Quaternion rot;

    private float lerpDuration = 1f;
    private float lerpRotationDuration = 0.5f;
    private bool changeCameraPosition;

    [HideInInspector]
    public GameObject currentCameraPosition;
    private GameObject previousCameraPosition;
    private Vector3 cameraDestination;

    private GameObject hitObject;

    void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        cameraContainer.transform.position = wakeUpPosition.transform.position;

        currentCameraPosition = startPosition;
        changeCameraPosition = false;

        StartCoroutine(ChangePositionSmooth(currentCameraPosition.transform.position));
    }



    void Update()
    {

        if (Input.touchCount > 0 && !changeCameraPosition)
        {
            Touch touch1 = Input.GetTouch(0);

            if (Input.touchCount < 2)
            {
                if (touch1.phase == TouchPhase.Began)
                {
                    RaycastChangePosition(touch1);

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

                StartCoroutine(GetComponent<ShowInformation>().RevealCanvas(currentCameraPosition.name));
                StartCoroutine(GetComponent<ShowInformation>().HideCanvas(previousCameraPosition.name));


                //previousCameraPosition.SetActive(true);
                //currentCameraPosition.SetActive(false);

            }
        }
    }

}
