using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private GameObject selectedEquipment;
    private GameObject selectedIngredient;
    private Camera playerCamera;
    private Vector3 originalPosition;

    [Tooltip("Height of object when picked up")]
    public float heightOffset;
    [Tooltip("Material of Syringe Needle before potion selected")]
    public Material emptySyringeNeedleMaterial;
    [Tooltip("Material of Sewing Needle before thread selected")]
    public Material emptySewingNeedleMaterial;

    public LayerMask equipmentLayer;
    public LayerMask ingredientLayer;
    public LayerMask ritualLayer;

    private void Start() {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update() {
        
        if (Input.GetMouseButtonDown(0)) {
            if (selectedEquipment == null) {
                RaycastHit hit = CastRay(equipmentLayer);

                if (hit.collider != null) {
                    Debug.Log("Picking up: " + hit.collider.gameObject.name);

                    selectedEquipment = hit.collider.gameObject;
                    originalPosition = selectedEquipment.transform.position;
                    Cursor.visible = false;
                }

            } else {
                if (selectedIngredient == null) {
                    RaycastHit hit = CastRay(ingredientLayer);

                    if (hit.collider != null) {
                        Debug.Log("Interacting with ingredient: " + hit.collider.gameObject.name);

                        if (selectedEquipment.CompareTag("SyringeNeedle") && hit.collider.gameObject.CompareTag("Potion")) {
                            selectedIngredient = hit.collider.gameObject;
                            selectedEquipment.GetComponent<MeshRenderer>().material = selectedIngredient.GetComponent<MeshRenderer>().material;
                        } else if (selectedEquipment.CompareTag("SewingNeedle") && hit.collider.gameObject.CompareTag("Thread")) {
                            selectedIngredient = hit.collider.gameObject;
                            selectedEquipment.GetComponent<MeshRenderer>().material = selectedIngredient.GetComponent<MeshRenderer>().material;
                        }
                        
                    } else {
                        Debug.Log("No ingredient detected");

                        selectedEquipment.transform.position = originalPosition;

                        selectedEquipment = null;
                        Cursor.visible = true;
                    }

                } else {
                    RaycastHit hit = CastRay(ritualLayer);

                    if (hit.collider != null) {

                        //ADD LOGIC FOR INTERACTING WITH DEAD BODY

                        Debug.Log("Interacting with ritual object: " + hit.collider.gameObject.name);
                        selectedIngredient = null;
                        if (selectedEquipment.CompareTag("SyringeNeedle")) {
                            selectedEquipment.GetComponent<MeshRenderer>().material = emptySyringeNeedleMaterial;
                        } else if (selectedEquipment.CompareTag("SewingNeedle")) {
                            selectedEquipment.GetComponent<MeshRenderer>().material = emptySewingNeedleMaterial;
                        }
                    }
                }


            }
        }

        if (selectedEquipment != null) {
            MoveObject(heightOffset);
        }
    }

    private void MoveObject(float heightOffset) {
        Vector3 position = new Vector3(
    Input.mousePosition.x,
    Input.mousePosition.y,
    playerCamera.WorldToScreenPoint(selectedEquipment.transform.position).z);
        Vector3 worldPosition = playerCamera.ScreenToWorldPoint(position);
        selectedEquipment.transform.position = new Vector3(worldPosition.x, heightOffset, worldPosition.z);
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
