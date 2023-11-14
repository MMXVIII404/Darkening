using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceExistingCubeOnPlane : MonoBehaviour
{
    public GameObject cubeToPlace; // 现有的Cube物体
    public ARRaycastManager raycastManager; // 用于进行射线检测的管理器

    bool a = true;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>(); // 获取ARRaycastManager组件
    }

    void Update()
    {
        // 在每一帧检测是否有平面
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon) && a)
        {
            // 如果检测到平面，将Cube物体放置在第一个检测到的平面上
            Pose hitPose = hits[0].pose;
            cubeToPlace.transform.position = hitPose.position;
            //cubeToPlace.
            a = false;
        }
    }
}
