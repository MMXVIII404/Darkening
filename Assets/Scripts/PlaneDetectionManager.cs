using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using System.Numerics;
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
    private int monsterCount;
    private int monsterCountBack;
    public GameObject trueMonsterPrefab;
    public GameObject smokeEffect;
    [SerializeField]
    private Camera mainCamera;
    private GameObject[] fakeMonsters;
    private int i = 0;
    public int maxMonsters = 6;
    public int trueMonsterNumber;
    private float deltaTime = 0.0f;
    public float maxGenerateHeight = 3.0f; //TODO: 改成预扫描要扫到天花板/实时更新当前最高的horzitonal plane + 2f为最高生成高度
    [SerializeField]
    private int currentMonsters = 0;
    private HashSet<ARPlane> processedPlanes = new HashSet<ARPlane>();
    private bool isButtonBegin;
    List<Vector2> boundaryPoints = new List<Vector2>();
    public GameObject preScanText;
    private bool firstInit = true;
    private GameObject firstFakeMonster;
    private bool allowInit = false;
    public Texture fakeMonsterMainTexture;
    public Texture trueMonsterMainTexture;
    private bool breakAllLoops = false;
    public AudioSource switchPositionAudioSource;
    public GameObject LeftRedImage;
    public GameObject RightRedImage;

    int count = 3;

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
        trueMonsterNumber = UnityEngine.Random.Range(2, maxMonsters - 1);
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
        if (fakeMonsters[trueMonsterNumber] != null)
        {
            Vector2 maincameraRightVector2 = new Vector2(mainCamera.transform.right.x,mainCamera.transform.forward.z).normalized;
            Vector2 maincameraForwardVector2 = new Vector2(mainCamera.transform.forward.x,mainCamera.transform.forward.z).normalized;

            Vector2 maincameraToTruemonsterVector2Normalized = new Vector2((fakeMonsters[trueMonsterNumber].transform.position - mainCamera.transform.position).x,
            (fakeMonsters[trueMonsterNumber].transform.position - mainCamera.transform.position).z).normalized;
            
            double angle = Vector2.Angle(maincameraForwardVector2,maincameraToTruemonsterVector2Normalized);
            double rightDotVector = Vector2.Dot(maincameraRightVector2,maincameraToTruemonsterVector2Normalized);
           
            // Debug.Log("Angle"+Vector2.Angle(maincameraRightVector2,maincameraToTruemonsterVector2Normalized));
            // Debug.Log("Dot"+Vector2.Dot(maincameraRightVector2,maincameraToTruemonsterVector2Normalized));

            
            if (rightDotVector >= 0 &&
            angle > 10 && 
            angle < 90)
            {
                RightRedImage.SetActive(true);
                LeftRedImage.SetActive(false);
            }
            else if (rightDotVector < 0 &&
            angle > 10 && 
            angle < 90)
            {
                RightRedImage.SetActive(false);
                LeftRedImage.SetActive(true);
            }
            
            else if(angle >= 90){
                RightRedImage.SetActive(true);
                LeftRedImage.SetActive(false);
            }
            else 
            {
                RightRedImage.SetActive(false);
                LeftRedImage.SetActive(false);
            }
        }
    }
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2f);
        if (firstFakeMonster != null)
        {
            firstFakeMonster.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            firstFakeMonster.transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            firstFakeMonster.transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
            firstFakeMonster.tag = "TrueMonster";
            // Destroy(firstFakeMonster);
            // Instantiate(trueMonsterPrefab, mainCamera.transform.position + mainCamera.transform.forward * 1f, mainCamera.transform.rotation);
            yield return new WaitForSeconds(0.5f);
            allowInit = true;
        }
        StopCoroutine(DelayDestroy());
    }
    public IEnumerator SwitchTrueMonster()
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
            else if (j == trueMonsterNumber)
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
            else
            {
                switchPositionAudioSource.PlayOneShot(switchPositionAudioSource.clip);
                fakeMonsters[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", trueMonsterMainTexture);
                yield return new WaitForSeconds(0.3f);
                fakeMonsters[j].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
                fakeMonsters[j].transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.SetTexture("_MainTex", fakeMonsterMainTexture);
            }
            
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
                if (planeManager.trackables.count >= 3) //plane.alignment == PlaneAlignment.HorizontalUp && 
                {
                    if (firstInit)
                    {
                        firstFakeMonster = Instantiate(fakeMonsterPrefab, mainCamera.transform.position + mainCamera.transform.forward * 1f, mainCamera.transform.rotation);
                        StartCoroutine(DelayDestroy());
                        firstInit = false;
                    }
                    if (allowInit) //plane.size.x > 1.5f && plane.size.y > 1.5f
                    {
                        count++;
                        foreach (var point in plane.boundary)
                        {
                            boundaryPoints.Add(point);
                        }
                        for (int i = 0; i < maxMonsters / 3; i++)
                        {
                            if (currentMonsters >= 1)
                            {
                                Vector2 randomPoint = RandomPointInPolygon(boundaryPoints);
                                float randomHeight = UnityEngine.Random.Range(0, maxGenerateHeight);
                                Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x,randomHeight, randomPoint.y));
                                // Debug.Log(randomPoint3D);
                                if (monsterCount >= 1)
                                {
                                    if (Vector3.Distance(randomPoint3D, fakeMonsters[monsterCount - 1].transform.position) < 0.3f)
                                    {
                                        randomPoint = RandomPointInPolygon(boundaryPoints);
                                        randomHeight = UnityEngine.Random.Range(0f, 0.1f+maxGenerateHeight);
                                        randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint.x, randomHeight, randomPoint.y));
                                    }
                                }
                                fakeMonsters[monsterCount] = Instantiate(fakeMonsterPrefab, randomPoint3D, mainCamera.transform.rotation);
                                monsterCount++;
                                currentMonsters -= 1;
                            }
                        }
                        if (currentMonsters == 0)
                        {
                            planeManager.enabled = false;
                            StartCoroutine(SwitchTrueMonster());
                            // if (count >= 3)
                            // {
                            allowInit = false;
                            // }
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
    Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x + 0.1f, bounds.max.x - 0.1f),
            UnityEngine.Random.Range(bounds.min.y + 0.1f, bounds.max.y - 0.1f)
        );
    }
    Vector2 RandomPointInPolygon(List<Vector2> poly)
    {
        try
        {
            // int maxAttempts = 100;
            // int attempts = 0;
            // 计算边界框
            Bounds bounds = new Bounds(poly[0], Vector2.zero);
            foreach (var point in poly)
            {
                bounds.Encapsulate(point);
            }
            Vector2 randomPoint = RandomPointInBounds(bounds);

            return randomPoint;
        }
        catch
        {
            return Vector2.zero;
        }
    }

    public void DestroyAllMonster()
    {
        for (int i = 0; i < fakeMonsters.Length; i++)
        {
            Destroy(fakeMonsters[i]);
        }
        Destroy(firstFakeMonster);

    }
}


