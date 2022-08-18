using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Camera arCamera;
    public GameObject startPosition;
    public GameObject wakeUpPosition;
    public GameObject cameraPositions;

    private int cameraPositionIndex;

    private GameObject cameraContainer;
    private Quaternion rot;

    private float lerpDuration = 1f;
    private float lerpRotationDuration = 0.5f;
    private bool changeCameraPosition;

    [HideInInspector]
    public GameObject currentCameraPosition;
    private GameObject previousCameraPosition;

    private ShowInformation showInformationComponent;

    private GameObject hitObject;


    void Start()
    {
        cameraContainer = new GameObject("Camera Container");
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        cameraContainer.transform.position = wakeUpPosition.transform.position;

        currentCameraPosition = startPosition;
        changeCameraPosition = false;

        cameraPositionIndex = currentCameraPosition.transform.GetSiblingIndex();

        showInformationComponent = GetComponent<ShowInformation>();

        StartCoroutine(ChangePositionSmooth(currentCameraPosition.transform.position));

        //StartCoroutine(showInformationComponent.RevealCanvas(currentCameraPosition.name));
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


        if (previousCameraPosition != null)
        {
            StartCoroutine(showInformationComponent.HideCanvas(previousCameraPosition.name));
        }


        while (timeElapsed < lerpDuration)
        {
            cameraContainer.transform.position = Vector3.Lerp(startPosition, cameraDestination, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        changeCameraPosition = false;
        cameraContainer.transform.position = cameraDestination;

        StartCoroutine(showInformationComponent.RevealCanvas(currentCameraPosition.name));

    }

    public void RaycastChangePosition(Touch touch)
    {
        Ray ray = arCamera.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.transform.gameObject;

            Debug.Log(hitObject);

            if (hitObject.tag == "StellarObject" && hitObject.name != currentCameraPosition.name)
            {
                previousCameraPosition = currentCameraPosition;
                currentCameraPosition = cameraPositions.transform.Find(hitObject.name).gameObject;

                Vector3 cameraDestination = currentCameraPosition.transform.position;
                StartCoroutine(ChangePositionSmooth(cameraDestination));

                cameraPositionIndex = currentCameraPosition.transform.GetSiblingIndex();

                //previousCameraPosition.SetActive(true);
                //currentCameraPosition.SetActive(false);

            }
        }
    }

    public void ChangeCameraPositionButtons(int index)
    {
        cameraPositionIndex += index;

        if (cameraPositionIndex < 1)
        {
            cameraPositionIndex = 1;
            return;
        }
        else if (cameraPositionIndex > 9)
        {
            cameraPositionIndex = 9;
            return;
        }

        previousCameraPosition = currentCameraPosition;
        currentCameraPosition = cameraPositions.transform.GetChild(cameraPositionIndex).gameObject;

        Vector3 cameraDestination = currentCameraPosition.transform.position;
        StartCoroutine(ChangePositionSmooth(cameraDestination));
    }

}
