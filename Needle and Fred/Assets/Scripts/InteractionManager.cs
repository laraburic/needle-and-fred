using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class InteractionManager : MonoBehaviour
{
    private GameObject selectedEquipment;
    private GameObject selectedIngredient;
    private GameObject catPaw;
    private Camera playerCamera;
    private Vector3 originalPosition;
    // highlight equipment logic
    private GameObject highlightedEquipment;
    private GameObject lastHighlightedEquipment;

    [Tooltip("Height of object when picked up")]
    public float heightOffset;
    [Tooltip("Material of Syringe Needle before potion selected")]
    public Material emptySyringeNeedleMaterial;
    [Tooltip("Material of Sewing Needle before thread selected")]
    public Material emptySewingNeedleMaterial;

    public LayerMask equipmentLayer;
    public LayerMask ingredientLayer;
    public LayerMask ritualLayer;
    public LayerMask allObjectsLayer;

    private GameObject puzzleManager;

    //=== START

    private void Start() {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        catPaw = GameObject.Find("CatPawCursor_Handle");
        puzzleManager = GameObject.Find("PuzzleManager");
    }

    //=== FUNCTIONS
    
    void ActivateHighlight() {
        if (highlightedEquipment.GetComponent<Outline>()) {
            highlightedEquipment.GetComponent<Outline>().enabled = true;
            lastHighlightedEquipment = highlightedEquipment;
        }
    }
    void DeactivateHighlight() {
        if (lastHighlightedEquipment != null) {
            if (lastHighlightedEquipment.GetComponent<Outline>() != null){
                lastHighlightedEquipment.GetComponent<Outline>().enabled = false;
            }
        }
    }

    void HighlightObjectsInLayer(LayerMask layer) {
        RaycastHit highlightHit = CastRay(layer);
        if (highlightHit.collider != null) {
            
            // For equipment object, the visible model is first child object ending in '_Model'
            if (layer == equipmentLayer) {
                highlightedEquipment = highlightHit.collider.gameObject.transform.GetChild(0).gameObject;
            } else {
                highlightedEquipment = highlightHit.collider.gameObject;
            }
            ActivateHighlight();
        } else {
            highlightedEquipment = null;
            DeactivateHighlight();
        }
    }

    void EnableOutlineObjects() {
        if (selectedEquipment == null) {
            HighlightObjectsInLayer(equipmentLayer);
        } else if (selectedIngredient == null) {
            HighlightObjectsInLayer(ingredientLayer);
        } else {
            HighlightObjectsInLayer(ritualLayer);
        }
    }
    
    //=== UPDATE

    private void Update() {
        // Function to get cat paw object to follow cursor
        FollowCursor(catPaw);

        // Highlight object functionality ----
        EnableOutlineObjects();
        // ----------------------------------

        if (Input.GetMouseButtonDown(0)) {
            if (selectedEquipment == null) {

                // If equipment is not currently being held, check for interaction with equipment
                RaycastHit hit = CastRay(equipmentLayer);

                if (hit.collider != null) {
                    Debug.Log("Picking up: " + hit.collider.gameObject.name);
                    selectedEquipment = hit.collider.gameObject;
                    originalPosition = selectedEquipment.transform.position;
                    FindObjectOfType<AudioManager>().Play("ToolPickup");
                }

            } else {
                if (selectedIngredient == null) {
                    // If equipment is currently being held, check for interaction with ingredients
                    RaycastHit hit = CastRay(ingredientLayer);

                    if (hit.collider != null) {
                        Debug.Log("Interacting with ingredient: " + hit.collider.gameObject.name);

                        // SyringeNeedle can only interact with Potions
                        if (selectedEquipment.CompareTag("SyringeNeedle") && hit.collider.gameObject.CompareTag("Potion")) {
                            selectedIngredient = hit.collider.gameObject;
                            selectedEquipment.GetComponentInChildren<MeshRenderer>().material = selectedIngredient.GetComponent<MeshRenderer>().material;
                            FindObjectOfType<AudioManager>().Play("LiquidPickup");
                            // If the object has child game objects, update all of the colours on the children as well 
                            if (selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>() != null) {
                                selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>().UpdateChildColours(selectedIngredient.GetComponent<MeshRenderer>().material);
                            }
                        } 

                        // SewingNeedle can only interact with Thread
                        else if (selectedEquipment.CompareTag("SewingNeedle") && hit.collider.gameObject.CompareTag("Thread")) {
                            selectedIngredient = hit.collider.gameObject;
                            selectedEquipment.GetComponentInChildren<MeshRenderer>().material = selectedIngredient.GetComponent<MeshRenderer>().material;
                           /* FindObjectOfType<AudioManager>().Play("");*/
                            // If the object has child game objects, update all of the colours on the children as well 
                            if (selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>() != null) {
                                selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>().UpdateChildColours(selectedIngredient.GetComponent<MeshRenderer>().material);
                            }
                        }
                        
                    } else {
                        // Put equipment back down on table if no ingredient clicked
                        Debug.Log("No ingredient detected");

                        selectedEquipment.transform.position = originalPosition;
                        selectedEquipment = null;
                        FindObjectOfType<AudioManager>().Play("ToolPutdown");
                    }

                } else {
                    // If equipment is currently being held AND ingredient has been selected, check for interaction with ritual objects
                    RaycastHit hit = CastRay(ritualLayer);

                    if (hit.collider != null) {

                        Debug.Log("Interacting with ritual object: " + hit.collider.gameObject.name);

                        if (hit.collider.gameObject.CompareTag("DeadBody")) {

                            puzzleManager.GetComponent<PuzzleManager>().CheckStep(selectedIngredient.GetComponent<IngredientComponent>().ingredientType);
                        }

                        // Ingredient selected with equipment is consumed upon use with any ritual object
                        selectedIngredient = null;
                        if (selectedEquipment.CompareTag("SyringeNeedle")) {
                            selectedEquipment.GetComponentInChildren<MeshRenderer>().material = emptySyringeNeedleMaterial;
                            FindObjectOfType<AudioManager>().Play("LiquidPutdown");
                            // If the object has child game objects, update all of the colours on the children as well 
                            if (selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>() != null) {
                                selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>().UpdateChildColours(emptySyringeNeedleMaterial);
                            }
                        } else if (selectedEquipment.CompareTag("SewingNeedle")) {
                            selectedEquipment.GetComponentInChildren<MeshRenderer>().material = emptySewingNeedleMaterial;
                            FindObjectOfType<AudioManager>().Play("SewingPutdown");
                            // If the object has child game objects, update all of the colours on the children as well 
                            if (selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>() != null) {
                                selectedEquipment.GetComponentInChildren<UpdateColoursOnChildren>().UpdateChildColours(emptySyringeNeedleMaterial);
                            }
                        }
                    }
                }
            }
        }

        // Pick up equipment at heightOffset if currently selected
        if (selectedEquipment != null) {
            FollowCursor(selectedEquipment);
        }
    }

    private void FollowCursor(GameObject objectToFollow) {
        if (objectToFollow) {
            Vector3 position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
                playerCamera.WorldToScreenPoint(objectToFollow.transform.position).z);
            Vector3 worldPosition = playerCamera.ScreenToWorldPoint(position);
            objectToFollow.transform.position = new Vector3(worldPosition.x, heightOffset, worldPosition.z);
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
