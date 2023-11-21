using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionManager : MonoBehaviour
{
    ARPlaneManager planeManager;
    public GameObject monster;
    private Camera mainCamera;
    private int i = 0;
    public Vector2 maxMonsters = new Vector2(1f, 1f);
    [SerializeField]
    private Vector2 currentMonsters = Vector2.zero;
    private HashSet<ARPlane> processedPlanes = new HashSet<ARPlane>();
    private bool test;

    void Start()
    {
        currentMonsters = maxMonsters;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        planeManager = GetComponent<ARPlaneManager>();
        if (planeManager != null)
        {
            // planeManager.planesChanged += OnPlanesChanged;
        }
    }
    private void Update()
    {
        if (currentMonsters.x >= 1)
        {
            Instantiate(monster, mainCamera.transform.position + new Vector3(0,0,1) * 2f, mainCamera.transform.rotation);
            currentMonsters.x -= 1;
            if (currentMonsters.x == 0)
            {

            }
        }
        else if (currentMonsters.y >= 1)
        {
            Instantiate(monster, mainCamera.transform.position - new Vector3(0,0,1) * 2f, mainCamera.transform.rotation);
            currentMonsters.y -= 1;
            if (currentMonsters.y == 0)
            {

            }
            // i++;
            // if (i > 240)
            // {
            //     Debug.Log(mainCamera.transform.position.y);
            //     i = 0;
            // }


        }

        // void OnPlanesChanged(ARPlanesChangedEventArgs args)
        // {
        //     foreach (var plane in args.added)
        //     {
        //         // Debug.Log(1);
        //         // 处理新检测到的平面
        //     }

        //     foreach (var plane in args.updated)
        //     {
        //         // if (!processedPlanes.Contains(plane))
        //         // {
        //         Debug.Log(plane.transform.position.z - mainCamera.transform.position.z);
        //         // Debug.Log("size:" + plane.size);
        //         // plane.alignmnt
        //         // if (plane.transform.position.y < mainCamera.transform.position.y - 0.8f && plane.size.x > 1.5f && plane.size.y > 1.5f)
        //         // {

        //         // plane.transform.position.y -= 0.3f;

        //         // Debug.Log("检测到地面");
        //         // }

        //         // }

        //         // 处理更新的平面
        //     }

        //     foreach (var plane in args.removed)
        //     {
        //         Debug.Log(3);
        //         // 处理移除的平面
        //     }
        // }
    }
}
