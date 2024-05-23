using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePosition;
    public static bool mouseButtonReleased;
    private Camera playerCamera;

    private void Start() {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    private Vector3 GetMousePos()
    {
        return playerCamera.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag()
    {
        transform.position = playerCamera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
    }

    private void OnMouseUp()
    {
        mouseButtonReleased = true;
    }

    //https://www.youtube.com/watch?v=9-ok9Cn3d90

    /*private void OnTriggerStay(Collider collison)
    {
        string thisGameObjectName;
        string collisionGameobjectName;

        thisGameObjectName = gameObject.Substring(0, name.IndexOf("_"));
        collisionGameobjectName = collisiion.gameObject.name.Substring(0, name.IndexOf("_"));

        if (mouseButtonReleased && thisGameObjectName == "Acorn" && thisGameObjectName == collisionGameobjectName
        {
            Instantiate(Resources.Load("Oak_Object")), transform.position, Quaternion.identity);
            mouseButtonReleased= false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (mouseButtonReleased && thisGameObjectName == "Oak" && thisGameObjectName == collisionGameobjectName
        {
            Instantiate(Resources.Load("Rocket_Object")), transform.position, Quaternion.identity);
            mouseButtonReleased = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }*/
}
