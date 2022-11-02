using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowInformation : MonoBehaviour
{
    public GameObject planetCanvases;
    public GameObject[] startingInformation;

    public Camera arCamera;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private bool showStartInformation;
    public int startInfoIndex;
    //private GameObject currentRevealCanvas;
    //private GameObject currentHideCanvas;

    //private Image currentImage;
    private float lerpDuration = 0.8f;

    //private TextMeshProUGUI[] textMeshesRevealCanvas;
    private TextMeshProUGUI[] textMeshesHideCanvas;

    private GameObject currentHitObject;
    private GameObject previousHitObject;

    private bool canvasRevealed;
    private bool couroutineOn;

    private float _alphaColor;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < planetCanvases.transform.childCount; i++)
        {
            planetCanvases.transform.GetChild(i).gameObject.SetActive(false);
        }

        startInfoIndex = 0;

        startingInformation[startInfoIndex].SetActive(false);

        StartCoroutine(DelayStartInformation());

        showStartInformation = true;

    }

    public IEnumerator DelayStartInformation()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ShowStartInformation());
    }

    // Update is called once per frame
    void Update()
    {

        if (showStartInformation == true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch1 = Input.GetTouch(0);

                if (Input.touchCount < 2)
                {
                    if (touch1.phase == TouchPhase.Began)
                    {
                        showStartInformation = false;
                        StartCoroutine(HideStartInformation());

                        audioSource.PlayOneShot(audioClip);
                    }
                }
            }
        }

        if (Input.GetKeyDown("z") && showStartInformation == true)
        {
            showStartInformation = false;
            StartCoroutine(HideStartInformation());
            audioSource.PlayOneShot(audioClip);
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
        GameObject currentRevealCanvas = planetCanvases.transform.Find(canvasName).gameObject;

        if (currentRevealCanvas != null)
        {
            yield return new WaitForSeconds(0.5f);

            TextMeshProUGUI[] textMeshesRevealCanvas = currentRevealCanvas.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI textMeshComponent in textMeshesRevealCanvas)
            {
                Color oldColor = textMeshComponent.color;

                textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
            }


            currentRevealCanvas.SetActive(true);

            Image currentImage = currentRevealCanvas.transform.GetComponentInChildren<Image>();

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

                foreach (TextMeshProUGUI textMeshComponent in textMeshesRevealCanvas)
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

    public IEnumerator HideCanvas(string newCurrentHideCanvas)
    {
        GameObject currentHideCanvas = planetCanvases.transform.Find(newCurrentHideCanvas).gameObject;

        if (currentHideCanvas != null)
        {

            Image currentImage = currentHideCanvas.transform.GetComponentInChildren<Image>();

            float timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;
                currentImage.fillAmount = 1 - timeElapsed / lerpDuration;
                yield return null;
            }

            TextMeshProUGUI[] textMeshesHideCanvas = currentHideCanvas.GetComponentsInChildren<TextMeshProUGUI>();

            timeElapsed = 0f;

            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;

                foreach (TextMeshProUGUI textMeshComponent in textMeshesHideCanvas)
                {
                    Color oldColor = textMeshComponent.color;

                    textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1 - timeElapsed / lerpDuration);
                }

                yield return null;
            }

            currentHideCanvas.SetActive(false);

            canvasRevealed = false;
            couroutineOn = false;
        }


    }

    public IEnumerator ShowStartInformation()
    {
        showStartInformation = false;

        float timeElapsed = 0f;

        TextMeshProUGUI[] startInformation = startingInformation[startInfoIndex].GetComponentsInChildren<TextMeshProUGUI>();
        Image[] startImages = startingInformation[startInfoIndex].GetComponentsInChildren<Image>();


        if (startInformation != null)
        {
            foreach (TextMeshProUGUI textMeshComponent in startInformation)
            {
                Color oldColor = textMeshComponent.color;

                textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
            }

            foreach (Image image in startImages)
            {
                _alphaColor = 0;

                Color oldColor = image.color;

                image.color = new Color(oldColor.r, oldColor.g, oldColor.b, _alphaColor);
            }

            startingInformation[startInfoIndex].SetActive(true);


            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;

                foreach (TextMeshProUGUI textMeshComponent in startInformation)
                {
                    Color oldColor = textMeshComponent.color;

                    textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, timeElapsed / lerpDuration);
                }

                foreach (Image image in startImages)
                {
                    if (image.transform.name == "Panel")
                    {
                        _alphaColor = 0.76f;
                    }
                    else
                    {
                        _alphaColor = 1f;
                    }

                    Color oldColor = image.color;

                    if (timeElapsed / lerpDuration < _alphaColor)
                    {
                        image.color = new Color(oldColor.r, oldColor.g, oldColor.b, timeElapsed / lerpDuration);
                    }

                }

                yield return null;
            }
        }

        showStartInformation = true;


    }

    public IEnumerator HideStartInformation()
    {

        float timeElapsed = 0f;

        TextMeshProUGUI[] startInformation = startingInformation[startInfoIndex].GetComponentsInChildren<TextMeshProUGUI>();
        Image[] startImages = startingInformation[startInfoIndex].GetComponentsInChildren<Image>();

        if (startInformation != null)
        {
            while (timeElapsed < lerpDuration)
            {
                timeElapsed += Time.deltaTime;

                foreach (TextMeshProUGUI textMeshComponent in startInformation)
                {
                    Color oldColor = textMeshComponent.color;

                    textMeshComponent.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1 - timeElapsed / lerpDuration);
                }

                foreach (Image image in startImages)
                {
                    if (image.transform.name == "Panel")
                    {
                        _alphaColor = 0.76f;
                    }
                    else
                    {
                        _alphaColor = 1f;
                    }

                    Color oldColor = image.color;

                    image.color = new Color(oldColor.r, oldColor.g, oldColor.b, _alphaColor - timeElapsed / lerpDuration);
                }

                yield return null;
            }
        }

        startingInformation[startInfoIndex].SetActive(false);

        startInfoIndex += 1;

        if (startInfoIndex < startingInformation.Length)
        {
            StartCoroutine(ShowStartInformation());
        }
    }

}
