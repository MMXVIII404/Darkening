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
    [SerializeField]
    private bool isButtonBegin;
    List<Vector2> boundaryPoints = new List<Vector2>();
    public void OnButtonBegin()
    {
        isButtonBegin = true;
    }

    void Start()
    {
        currentMonsters = maxMonsters;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        planeManager = GetComponent<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
    }
    private void Update()
    {
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            // Debug.Log(1);
            // 处理新检测到的平面
        }

        foreach (var plane in args.updated)
        {
            // if (!processedPlanes.Contains(plane))
            // {
            // Debug.Log(plane.transform.position.z - mainCamera.transform.position.z);
            // Debug.Log("size:" + plane.size);
            // plane.alignmnt
            // if (plane.transform.position.y < mainCamera.transform.position.y - 0.8f && plane.size.x > 1.5f && plane.size.y > 1.5f)
            // {

            // plane.transform.position.y -= 0.3f;
            if (isButtonBegin)
            {
                if (plane.size.x > 1.5f && plane.size.y > 1.5f)
                {
                    if (currentMonsters.x >= 1 && plane.transform.position.z - mainCamera.transform.position.z > 0)
                    {
                        foreach (var point in plane.boundary)
                        {
                            boundaryPoints.Add(point);
                        }
                        Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                        Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, 0, randomPoint.y));
                        Instantiate(monster, randomPoint3D, mainCamera.transform.rotation);
                        currentMonsters.x -= 1;
                        if (currentMonsters.x == 0 && currentMonsters.y == 0)
                        {
                            isButtonBegin = false;
                        }
                    }
                    else if (currentMonsters.y >= 1 && plane.transform.position.z - mainCamera.transform.position.z < 0)
                    {
                        foreach (var point in plane.boundary)
                        {
                            boundaryPoints.Add(point);
                        }
                        Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                        Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, 0, randomPoint.y));
                        Instantiate(monster, mainCamera.transform.position - new Vector3(0, 0, 1) * 2f, mainCamera.transform.rotation);
                        currentMonsters.y -= 1;
                        if (currentMonsters.x == 0 && currentMonsters.y == 0)
                        {
                            isButtonBegin = false;
                        }

                    }
                }

            }

        }

        foreach (var plane in args.removed)
        {
            // Debug.Log(3);
            // 处理移除的平面
        }
    }
    bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        bool inside = false;
        for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
        {
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }
    Vector2 RandomPointInPolygon(List<Vector2> poly)
    {
        // 计算边界框
        Bounds bounds = new Bounds(poly[0], Vector3.zero);
        foreach (var point in poly)
        {
            bounds.Encapsulate(point);
        }

        // 尝试随机点，直到找到一个在多边形内的点
        while (true)
        {
            Vector2 randomPoint = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (IsPointInPolygon(randomPoint, poly))
            {
                return randomPoint;
            }
        }
    }

}


