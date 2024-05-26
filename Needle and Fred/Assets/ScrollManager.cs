using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public bool scrollOpen;
    private Camera playerCamera;
    public GameObject audio;
    public bool dontTriggerAtStart = false;
    public GameObject scrollUI;
    public FreezeCamera freezeCam;
    //public LayerMask scrollLayer; // for opening and closing the scroll

    private void Start() {
        freezeCam.DeactivateNoise();
        scrollOpen = true;
        scrollUI.SetActive(true);
    }
    public void PlayAudio() {
        //audio.GetComponent<AudioSource>().Play();
    }

    public void OpenScroll()
    {
        scrollOpen = true;
        scrollUI.SetActive(true);
        freezeCam.DeactivateNoise();
    }
    
    public void CloseScroll()
    {
        scrollUI.SetActive(false);
        freezeCam.ReactivateNoise();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {

            /*// IF SCROLL ISNT OPEN
            if (scrollOpen != true) {
                RaycastHit scrollHit = CastRay(scrollLayer);
                if (scrollHit.collider != null)
                {
                    Debug.Log("Clicked on scroll");
                    scrollOpen = true;
                    
                    //PlayAudio();
                    
                }
            }
            // IF SCROLL IS OPEN
            if (scrollOpen == true) {
                scrollOpen = false;
                scrollUI.SetActive(false);
                //PlayAudio();
                
            }*/
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
