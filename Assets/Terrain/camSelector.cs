using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class camSelector : MonoBehaviour
{

    public Camera drawingCamera;
    public Camera explorationCamera;
    private Camera smallCam;
    private Rect smallSize = new Rect(.7f, .7f, 1f, 1f);
    private Rect fullSize = new Rect(0f, 0f, 1f, 1f);
    public GameObject player;
    FirstPersonController controller;
    public GameObject tracer;
    Tracer traceTool;

    void Start()
    {
        smallCam = explorationCamera;
        controller = player.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButtonUp(0)) && smallCam.pixelRect.Contains(Input.mousePosition))
            resizeCamera();
    }

    void resizeCamera()
    {
        if (smallCam == drawingCamera)
        {
            controller.toggleMouseLook();
            drawingCamera.rect = fullSize;
            explorationCamera.rect = smallSize;
            smallCam = explorationCamera;
        }
        else if (smallCam == explorationCamera)
        {
            controller.toggleMouseLook();
            traceTool = tracer.GetComponent<Tracer>();
            traceTool.unsetToggles();
            explorationCamera.rect = fullSize;
            drawingCamera.rect = smallSize;
            smallCam = drawingCamera;
        }
    }
}