using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlanet : MonoBehaviour
{
    public Transform objectToFollow;
    Vector3 _followOffset;

    public const int orbitSample = 100;
    public static bool drawOrbit = false;
    public const float scaleFactor = 0.0001f;

    private Quaternion _moveAroundQuaternion;
    private float rotationMoveSpeed = 1f;

    private bool isObjectCanvas;
    private Transform sceneCamera;

    [HideInInspector]
    public bool rotationButtonIsPressed;

    private void Awake()
    {
        _followOffset = transform.position - objectToFollow.position;
        _moveAroundQuaternion = Quaternion.Euler(0, 0, 0);
    }

    private void Start()
    {
        rotationButtonIsPressed = false;

        if (transform.childCount > 1)
        {
            isObjectCanvas = true;
        }
        else
        {
            isObjectCanvas = false;
        }

        sceneCamera = GameObject.Find("Main Camera").transform;

    }


    void Update()
    {

        if (rotationButtonIsPressed)
        {
            _moveAroundQuaternion *= Quaternion.Euler(0, rotationMoveSpeed, 0);
        }



        transform.position = _moveAroundQuaternion * _followOffset + objectToFollow.position;

        if (isObjectCanvas)
        {
            transform.LookAt(sceneCamera);

            transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            transform.Rotate(0, 180, 0);

        }
    }

    public void RotationButtonPressed(bool buttonStatus)
    {
        rotationButtonIsPressed = buttonStatus;
    }

}
