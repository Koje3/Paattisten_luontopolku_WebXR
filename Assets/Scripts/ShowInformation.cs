using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInformation : MonoBehaviour
{
    public GameObject canvases;
    public Camera arCamera;

    private GameObject currentCanvas;
    private Image currentImage;
    private float lerpDuration = 0.8f;

    private TextMeshProUGUI[] textMeshes;

    private GameObject currentHitObject;
    private GameObject previousHitObject;

    private bool canvasRevealed;
    private bool couroutineOn;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < canvases.transform.childCount; i++)
        {
            canvases.transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(RevealCanvas("Earth"));
        }



        //int layerMask = 1 << 5;
        //layerMask = ~layerMask;

        //Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.forward);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        //{
        //    currentHitObject = hit.transform.gameObject;

        //    Debug.Log(currentHitObject);

        //    if (currentHitObject.tag == "StellarObject" && previousHitObject != currentHitObject && currentHitObject.name == GetComponent<MoveCamera>().currentCameraPosition.name && currentHitObject != null && couroutineOn == false)
        //    {
        //        couroutineOn = true;
        //        previousHitObject = currentHitObject;
        //        StartCoroutine(RevealCanvas(currentHitObject.name));
        //    }

        //    if (currentHitObject == null && canvasRevealed == true && couroutineOn == false)
        //    {
        //        couroutineOn = true;
        //        StartCoroutine(HideCanvas(currentHitObject.name));
        //    }

        //}  

    }

    public IEnumerator RevealCanvas(string canvasName)
    {
        currentCanvas = canvases.transform.Find(canvasName).gameObject;

        if (currentCanvas != null)
        {
            yield return new WaitForSeconds(2);

            textMeshes = currentCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textMeshComponent in textMeshes)
            {
                Color oldColor = textMeshComponent.color;

                textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
            }


            currentCanvas.SetActive(true);

            Image currentImage = currentCanvas.transform.GetComponentInChildren<Image>();

            float timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                currentImage.fillAmount = timeElapsed / lerpDuration;
                yield return null;
            }


            timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;

                foreach (TextMeshProUGUI textMeshComponent in textMeshes)
                {
                    Color oldColor = textMeshComponent.color;

                    textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, timeElapsed / lerpDuration);
                }

                yield return null;
            }

            canvasRevealed = true;
            couroutineOn = false;
        }
    }

    public IEnumerator HideCanvas(string newCurrentCanvas = null)
    {
        if (newCurrentCanvas != null)
        {
            currentCanvas = canvases.transform.Find(newCurrentCanvas).gameObject;
        }

        if (currentCanvas != null)
        {

            Image currentImage = currentCanvas.transform.GetComponentInChildren<Image>();

            float timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                currentImage.fillAmount = 1 - timeElapsed / lerpDuration;
                yield return null;
            }

            textMeshes = currentCanvas.GetComponentsInChildren<TextMeshProUGUI>();

            timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;

                foreach (TextMeshProUGUI textMeshComponent in textMeshes)
                {
                    Color oldColor = textMeshComponent.color;

                    textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1 - timeElapsed / lerpDuration);
                }

                yield return null;
            }

            canvasRevealed = false;
            couroutineOn = false;
        }


    }

}
