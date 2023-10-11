using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using Vector2 = UnityEngine.Vector2;

public class ARInteractionManeger : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject aRPointer;
    private GameObject item3DModel;
    private bool isInitialPosition;
    private bool isOverUI;

    public GameObject Item3DModel
    {

        set
        {
         item3DModel = value;
        item3DModel.transform.position = aRPointer.transform.position;
        item3DModel.transform.parent = aRPointer.transform;
        isInitialPosition = true;  
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        aRRaycastManager = FindObjectOfType <ARRaycastManager>();
        GameManager.instance.OnMainMenu += SetItemPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);

            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                aRPointer.SetActive(true);
                isInitialPosition = false;  

            }



        }


        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == UnityEngine.TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                isOverUI = isTapOverUI(touchPosition);
            }
            if (touchOne.phase == UnityEngine.TouchPhase.Moved)
            {
                if (aRRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    if (!isOverUI)
                    {
                        transform.position = hitPose.position;
                    }
                }
            }
        }

    }

    private bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;

    }

    private void SetItemPosition()

    {
       if (item3DModel != null)
        {
            item3DModel.transform.parent = null;
            aRPointer.SetActive(false);
            item3DModel = null;
        }



    }

    public void DeleteItem()
    {
        Destroy(item3DModel);
        aRPointer.SetActive(false);
        GameManager.instance.MainMenu();
    }

}
