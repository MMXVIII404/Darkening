using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlaneTransparent : MonoBehaviour
{
    public Material transparentMaterial;
    public Renderer planeRenderer;
    GameObject trackables;
    public void SetMaterial()
    {
        trackables = GameObject.Find("Trackables");
        foreach (Renderer renderer in trackables.GetComponentsInChildren<Renderer>())
        {
            renderer.material = transparentMaterial;
        }
        planeRenderer.material = transparentMaterial;
    }
}
