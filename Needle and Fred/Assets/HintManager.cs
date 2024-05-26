using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public List<GameObject> textToOverlay;
    public GameObject hint;
    public GameObject controls;
    public TMP_Text textHint;
    public string notHoldingEquipment;
    public string holdingEquipment;
    public string hasIngredient;
    private InteractionManager _interactionManager;
    
    // Set up all the references
    private void Start() {
        Setup();
    }
    
    // On H key press, toggle visibility of help text
    private void Update() {
        if(Input.GetKeyDown(KeyCode.H)) {
            ToggleOverlay();
        }
    }
    
    void ToggleOverlay() {
        
        // Equipment text overlay
        foreach (GameObject o in textToOverlay) {
            o.SetActive(!o.activeInHierarchy); // set active to be opposite of whatever it is now
        }
        // Hint box
        hint.SetActive(!hint.activeInHierarchy);
    }

    public void UpdateText(string text) {
        textHint.SetText("");
        textHint.SetText(text);
    }
    
    void Setup() {
        
        // InteractionManager
        if (_interactionManager == null) {
            _interactionManager = GameObject.FindObjectOfType<InteractionManager>();
            if (_interactionManager != null) {
                textHint.SetText(notHoldingEquipment);
            }
            else {
                Debug.LogError("Missing InteractionManager in HintManager");
            }
        }
        
        // Controls
        if (controls != null) {
            controls.SetActive(true); // always make visible
        }
        else {
            Debug.LogError("Missing controls GameObject in HintManager");
        }
        
        // Hint box
        if (hint != null) {
            hint.SetActive(false); // set to hidden at start
        }
        else {
            Debug.LogError("Missing hint GameObject in HintManager");
        }
        
        // Equipment text overlay
        if (textToOverlay.Count != 0) {
            foreach (GameObject o in textToOverlay) {
                o.SetActive(false);
            }
        }
        else {
            Debug.LogError("Missing textToOverlay GameObjects in HintManager");
        }
    }
}
