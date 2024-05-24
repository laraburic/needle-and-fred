using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateColoursOnChildren : MonoBehaviour
{
    private MeshRenderer[] childMeshRenderers;
    private void Start() {
        childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void UpdateChildColours(Material material) {
        foreach (MeshRenderer meshRenderers in childMeshRenderers) {
            meshRenderers.material = material;
        }
    }
}
