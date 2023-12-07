using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectionManager : MonoBehaviour
{
    ARPlaneManager planeManager;
    public GameObject fakeMonsterPrefab;
    [SerializeField]
    private int monsterCountFront;
    [SerializeField]
    private int monsterCountBack;
    public GameObject trueMonsterPrefab;
    public GameObject smokeEffect;
    private Camera mainCamera;
    [SerializeField]
    private GameObject[] fakeMonstersFront;
    [SerializeField]
    private GameObject[] fakeMonstersBack;
    private int i = 0;
    public Vector2 maxMonsters = new Vector2(25f, 25f);
    [SerializeField]
    private int trueMonsterNumber;
    [SerializeField]
    private bool forwardOrBack = true;
    [SerializeField]
    private float monsterStart = 0.0f;
    private int monsterStartCount = 0;
    private bool addOnce = true;
    private float deltaTime = 0.0f;
    public float maxGenerateHeight = 3.0f; //TODO: 改成预扫描要扫到天花板/实时更新当前最高的horzitonal plane + 2f为最高生成高度
    [SerializeField]
    private Vector2 currentMonsters = Vector2.zero;
    private HashSet<ARPlane> processedPlanes = new HashSet<ARPlane>();
    [SerializeField]
    private bool isButtonBegin;
    List<Vector2> boundaryPoints = new List<Vector2>();
    public GameObject preScanText;
    public void OnButtonBegin()
    {
        isButtonBegin = true;
        preScanText.SetActive(false);
        if (smokeEffect != null)
        {
            // Debug.Log(1);
            smokeEffect.GetComponent<ParticleSystemRenderer>().enabled = true;
        }

    }

    void Start()
    {
        smokeEffect.GetComponent<ParticleSystemRenderer>().enabled = false;
        preScanText.SetActive(true);
        fakeMonstersFront = new GameObject[(int)maxMonsters.x * 2];
        fakeMonstersBack = new GameObject[(int)maxMonsters.y * 2];
        trueMonsterNumber = Random.Range(3, (int)maxMonsters.x * 2 - 1);
        forwardOrBack = (trueMonsterNumber - (int)maxMonsters.x) >= 0 ? false : true;
        if (!forwardOrBack)
        {
            trueMonsterNumber -= (int)maxMonsters.x;
        }
        addOnce = true;
        monsterStartCount = 0;
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
                if (plane.alignment == PlaneAlignment.HorizontalUp)
                {
                    if (true) //plane.size.x > 1.5f && plane.size.y > 1.5f
                    {
                        //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
                        //float fps = 1.0f / deltaTime;
                        monsterStart++;
                        if (monsterStart > 60)
                        {
                            monsterStart = 0.0f;
                            addOnce = true;
                            if (addOnce)
                            {
                                monsterStartCount++;
                                addOnce = false;
                            }
                            for (int i = 0; i < monsterStartCount; i++)
                            {
                                if (plane.transform.position.z - mainCamera.transform.position.z > 0)
                                {
                                    if (currentMonsters.x >= 1)
                                    {
                                        foreach (var point in plane.boundary)
                                        {
                                            boundaryPoints.Add(point);
                                        }
                                        Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                                        float randomHeight = Random.Range(0, maxGenerateHeight);
                                        Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, randomHeight, randomPoint.y));
                                        Debug.Log(randomPoint3D);
                                        fakeMonstersFront[monsterCountFront] = Instantiate(fakeMonsterPrefab, randomPoint3D, mainCamera.transform.rotation);
                                        monsterCountFront++;
                                        currentMonsters.x -= 1;
                                        if (currentMonsters.x == 0)
                                        {
                                            if (forwardOrBack)
                                            {
                                                Debug.Log("1");
                                                /* if (trueMonsterNumber >= 1)
                                                 {
                                                     Destroy(fakeMonstersFront[trueMonsterNumber - 1]);
                                                 }
                                                 else
                                                 {
                                                     Destroy(fakeMonstersFront[0]);
                                                 }*/
                                                Instantiate(trueMonsterPrefab, fakeMonstersFront[trueMonsterNumber - 1].transform.position, mainCamera.transform.rotation);
                                                Destroy(fakeMonstersFront[trueMonsterNumber - 1]);

                                            }
                                            //isButtonBegin = false;
                                        }
                                    }

                                }
                                else if (plane.transform.position.z - mainCamera.transform.position.z < 0)
                                {
                                    if (currentMonsters.y >= 1)
                                    {
                                        foreach (var point in plane.boundary)
                                        {
                                            boundaryPoints.Add(point);
                                        }
                                        Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                                        float randomHeight = Random.Range(0, maxGenerateHeight);
                                        Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, randomHeight, randomPoint.y));
                                        fakeMonstersBack[monsterCountBack] = Instantiate(fakeMonsterPrefab, mainCamera.transform.position - new Vector3(0, 0, 1) * 2f, mainCamera.transform.rotation);
                                        monsterCountBack++;
                                        currentMonsters.y -= 1;
                                        if (currentMonsters.y == 0)
                                        {
                                            if (!forwardOrBack)
                                            {
                                                Debug.Log("2");
                                                /*if (trueMonsterNumber >= 1)
                                                {
                                                    Destroy(fakeMonstersBack[trueMonsterNumber - 1]);
                                                }
                                                else
                                                {
                                                    Destroy(fakeMonstersBack[0]);
                                                }*/
                                                //Instantiate(trueMonsterPrefab, randomPoint3D, mainCamera.transform.rotation);
                                                Destroy(fakeMonstersBack[trueMonsterNumber - 1]);
                                                Instantiate(trueMonsterPrefab, fakeMonstersBack[trueMonsterNumber - 1].transform.position, mainCamera.transform.rotation);
                                            }
                                            //isButtonBegin = false;
                                        }
                                    }
                                }
                                if (i == monsterStartCount - 1)
                                {  //这里可能没有-1，具体需要调试
                                   //monsterStart = 0.0f;
                                   //addOnce = true;
                                }
                            }
                            //monsterStart = 0.0f;
                            //addOnce = true;


                        }

                    }
                }
                else if (plane.alignment == PlaneAlignment.Vertical)
                {

                    // TODO : generate vine effect;
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


