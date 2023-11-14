using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceExistingCubeOnPlane : MonoBehaviour
{
    public GameObject cubeToPlace; // ���е�Cube����
    public ARRaycastManager raycastManager; // ���ڽ������߼��Ĺ�����

    bool a = true;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>(); // ��ȡARRaycastManager���
    }

    void Update()
    {
        // ��ÿһ֡����Ƿ���ƽ��
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon) && a)
        {
            // �����⵽ƽ�棬��Cube��������ڵ�һ����⵽��ƽ����
            Pose hitPose = hits[0].pose;
            cubeToPlace.transform.position = hitPose.position;
            //cubeToPlace.
            a = false;
        }
    }
}
