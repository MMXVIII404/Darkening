using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionManager : MonoBehaviour
{
    ARPlaneManager planeManager;
    public GameObject monster;
    private HashSet<ARPlane> processedPlanes = new HashSet<ARPlane>(); 

    void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            Debug.Log(1);
            // 处理新检测到的平面
        }

        foreach (var plane in args.updated)
        {
            if(!processedPlanes.Contains(plane)){
            processedPlanes.Add(plane);
            Instantiate(monster, plane.transform.position, plane.transform.rotation);
            monster.transform.up = plane.transform.up;
            Debug.Log("检测到Vertical平面");
            }
            
            // 处理更新的平面
        }

        foreach (var plane in args.removed)
        {
            Debug.Log(3);
            // 处理移除的平面
        }
    }
}
