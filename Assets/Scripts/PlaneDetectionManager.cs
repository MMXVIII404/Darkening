using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetectionManager : MonoBehaviour
{
    ARPlaneManager planeManager;
    public GameObject fakeMonsterPrefab;
    [SerializeField]
    private int monsterCount;
    [SerializeField]
    private int monsterCountBack;
    public GameObject trueMonsterPrefab;
    public GameObject smokeEffect;
    private Camera mainCamera;
    [SerializeField]
    private GameObject[] fakeMonsters;
    [SerializeField]
    private int i = 0;
    public int maxMonsters = 6;
    [SerializeField]
    private int trueMonsterNumber;
    [SerializeField]
    private float monsterStart = 0.0f;
    private int monsterStartCount = 0;
    private bool addOnce = true;
    private float deltaTime = 0.0f;
    public float maxGenerateHeight = 3.0f; //TODO: 改成预扫描要扫到天花板/实时更新当前最高的horzitonal plane + 2f为最高生成高度
    [SerializeField]
    private int currentMonsters = 0;
    private HashSet<ARPlane> processedPlanes = new HashSet<ARPlane>();
    [SerializeField]
    private bool isButtonBegin;
    List<Vector2> boundaryPoints = new List<Vector2>();
    public GameObject preScanText;
    private bool firstInit = true;
    [SerializeField]
    private GameObject firstFakeMonster;
    private bool allowInit = false;
    public Texture fakeMonsterMainTexture;
    public Texture trueMonsterMainTexture;
    private bool breakAllLoops = false;
    public AudioSource switchPositionAudioSource;

    private Transform smokeEffectPosition;
    public void OnButtonBegin()
    {
        isButtonBegin = true;
        preScanText.SetActive(false);
        smokeEffect.transform.position = new Vector3(0, 0, 0);
        // if (smokeEffect != null)
        // {
        //     // Debug.Log(1);
        //     smokeEffect.GetComponent<ParticleSystemRenderer>().enabled = true;
        // }

    }

    void Start()
    {
        firstInit = true;
        allowInit = false;
        smokeEffect.transform.position = new Vector3(1000, 1000, 1000);
        // smokeEffect.GetComponent<ParticleSystemRenderer>().enabled = false;
        preScanText.SetActive(true);
        fakeMonsters = new GameObject[maxMonsters * 2];
        trueMonsterNumber = Random.Range(2, maxMonsters - 1);
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
        // Debug.Log(planeManager.trackables.count);
    }
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2f);
        firstFakeMonster.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
        firstFakeMonster.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
        firstFakeMonster.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
        firstFakeMonster.tag = "TrueMonster";
        // Destroy(firstFakeMonster);
        // Instantiate(trueMonsterPrefab, mainCamera.transform.position + mainCamera.transform.forward * 1f, mainCamera.transform.rotation);
        allowInit = true;
    }
    private IEnumerator SwitchTrueMonster()
    {
        for (int j = 0; j < monsterCount; j++)
        {
            if (j == 0)
            {
                firstFakeMonster.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
                firstFakeMonster.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
                firstFakeMonster.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
                firstFakeMonster.tag = "Monster";
            }
            if (j == trueMonsterNumber)
            {
                fakeMonsters[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                fakeMonsters[j].tag = "TrueMonster";
                // Transform transformBackup = fakeMonsters[j].transform;
                // Destroy(fakeMonsters[j]);
                // Instantiate(trueMonsterPrefab, transformBackup.position, transformBackup.rotation);
                // breakAllLoops = true;
                break;
            }
            // if(breakAllLoops){
            //     break;
            // }
            switchPositionAudioSource.PlayOneShot(switchPositionAudioSource.clip);
            fakeMonsters[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            fakeMonsters[j].transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            fakeMonsters[j].transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            yield return new WaitForSeconds(0.5f);
            fakeMonsters[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
            fakeMonsters[j].transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
            fakeMonsters[j].transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
            if (j == monsterCount - 1)
            {
                StopCoroutine(SwitchTrueMonster());
            }
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
        }

        foreach (var plane in args.updated)
        {
            if (isButtonBegin)
            {
                if (planeManager.trackables.count >= 2) //plane.alignment == PlaneAlignment.HorizontalUp && 
                {
                    if (firstInit)
                    {
                        Debug.Log(111);
                        firstFakeMonster = Instantiate(fakeMonsterPrefab, mainCamera.transform.position + mainCamera.transform.forward * 1f, mainCamera.transform.rotation);
                        StartCoroutine(DelayDestroy());
                        firstInit = false;
                    }
                    if (allowInit) //plane.size.x > 1.5f && plane.size.y > 1.5f
                    {
                        monsterStart++;
                        if (monsterStart > 45)
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
                                if (currentMonsters >= 1)
                                {
                                    foreach (var point in plane.boundary)
                                    {
                                        boundaryPoints.Add(point);
                                    }
                                    Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                                    float randomHeight = Random.Range(0, maxGenerateHeight);
                                    Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, randomHeight, randomPoint.y));
                                    // Debug.Log(randomPoint3D);
                                    fakeMonsters[monsterCount] = Instantiate(fakeMonsterPrefab, randomPoint3D, mainCamera.transform.rotation);
                                    monsterCount++;
                                    currentMonsters -= 1;
                                }
                            }
                            if (currentMonsters == 0)
                            {
                                planeManager.enabled = false;
                                // Debug.Log("1");
                                StartCoroutine(SwitchTrueMonster());
                                // allowInit = false;
                            }
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


