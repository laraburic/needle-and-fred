using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpOverlay : MonoBehaviour
{
    public List<GameObject> textToOverlay;

    // Set help text to be disabled by default
    private void Start() {
        foreach (GameObject o in textToOverlay) {
            o.SetActive(false);
        }
    }

    // On H key press, toggle visibility of help text
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H)) {
            ToggleOverlay();
            Debug.Log("Pressed H");
        }
    }

    void ToggleOverlay() {
        foreach (GameObject o in textToOverlay) {
            o.SetActive(!o.activeInHierarchy); // set active to be opposite of whatever it is now
        }
    }
}
