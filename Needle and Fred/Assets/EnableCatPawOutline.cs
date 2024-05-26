using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCatPawOutline : MonoBehaviour
{
    private Outline catPawOutline;

    private void Awake()
    {
        catPawOutline = this.GetComponent<Outline>();
    }

    // Start is called before the first frame update
    void Start() {
        catPawOutline.enabled = true;
    }
}
