using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MoveCamera : MonoBehaviour
{
    public Camera arCamera;
    public GameObject startPosition;
    public GameObject wakeUpPosition;
    public GameObject cameraPositions;
    public TextMeshProUGUI planetText;
    public GameObject planetCanvases;

    public AudioSource audioSource;
    public AudioClip audioClip;

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

    private bool smoothPositionChangeCompleted;


    void Start()
    {
        smoothPositionChangeCompleted = false;

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

        if (currentCameraPosition != null && smoothPositionChangeCompleted)
        {
            cameraContainer.transform.position = currentCameraPosition.transform.position;
        }

    }

    public IEnumerator ChangePositionSmooth(Vector3 cameraDestination)
    {
        SetPlanetNameText(currentCameraPosition.name);

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

        smoothPositionChangeCompleted = true;

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
                audioSource.PlayOneShot(audioClip);

                smoothPositionChangeCompleted = false;

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

        smoothPositionChangeCompleted = false;

        previousCameraPosition = currentCameraPosition;
        currentCameraPosition = cameraPositions.transform.GetChild(cameraPositionIndex).gameObject;

        Vector3 cameraDestination = currentCameraPosition.transform.position;
        StartCoroutine(ChangePositionSmooth(cameraDestination));
    }

    public void RotateCameraAndCanvasAround(bool buttonStatus)
    {
        currentCameraPosition.GetComponent<FollowPlanet>().RotationButtonPressed(buttonStatus);

        planetCanvases.transform.Find(currentCameraPosition.name).GetComponent<FollowPlanet>().RotationButtonPressed(buttonStatus);
    }


    void SetPlanetNameText(string planetName)
    {
        switch (planetName)
        {
            case "Sun":
                planetText.text = "AURINKO";
                break;

            case "Mercury":
                planetText.text = "MERKURIUS";
                break;

            case "Venus":
                planetText.text = "VENUS";
                break;

            case "Earth":
                planetText.text = "MAA";
                break;

            case "Mars":
                planetText.text = "MARS";
                break;

            case "Jupiter":
                planetText.text = "JUPITER";
                break;

            case "Saturn":
                planetText.text = "SATURNUS";
                break;

            case "Neptune":
                planetText.text = "NEPTUNUS";
                break;

            case "Uranus":
                planetText.text = "URANUS";
                break;

            default:
                break;
        }
    }

}
