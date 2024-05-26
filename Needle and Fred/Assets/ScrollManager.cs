using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    private Camera playerCamera;
    public bool dontTriggerAtStart = false;
    public GameObject scrollUI;
    public FreezeCamera freezeCam;
    public LayerMask scrollLayer; // for opening and closing the scroll

    private void Start() {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (Application.isEditor) {
            if (dontTriggerAtStart) {
                scrollUI.SetActive(false);
            }
        }
        if (Application.isEditor){
            if (!dontTriggerAtStart) {
                scrollUI.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        if (!Application.isEditor) {
            scrollUI.SetActive(true);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit scrollHit = CastRay(scrollLayer);
            if (scrollHit.collider != null) {
                scrollUI.SetActive(true);
                foreach (Transform t in scrollUI.GetComponentInChildren<Transform>()) {
                    t.gameObject.SetActive(true);
                }
                freezeCam.DeactivateNoise();
            }
        }
    }
    private RaycastHit CastRay(LayerMask layer) {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x, 
            Input.mousePosition.y, 
            playerCamera.farClipPlane);        
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x, 
            Input.mousePosition.y, 
            playerCamera.nearClipPlane);
        Vector3 worldMousePosFar = playerCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = playerCamera.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Mathf.Infinity, layer);

        return hit;
    }
}
