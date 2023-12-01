using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AimOnTarget : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 3f;
    public GameObject displayText;

    public Image checkArea;
    public event Action onHitAction;

    bool buttonPressed = false;
    bool inRange = false;
    string currentMonsterName = string.Empty;

    float timeCount = 0;
    bool startCountTime = false;
    bool catched = false;
    bool startCatching = false;
    int San = 100;

    //public Button catchButton;

    void Start()
    {
        // mainCamera = Camera.main;
    }

    void Update()
    {
        //Catch();
        CountTime();
        PointAtMonster();
    }


    public void SetCatchStatus()
    {
        if (inRange)
        {
            buttonPressed = true;
        }
    }


    private bool IsPointInsideCheckArea(GameObject Monster)
    {
        Vector2 checkAreaPostion = RectTransformUtility.WorldToScreenPoint(mainCamera, Monster.transform.position);    // Monster postion on screen
        checkArea.GetComponent<RectTransform>().position = checkAreaPostion;   // Check area follow the Monster position on screen

        float distanceToCheckArea = Vector2.Distance(checkAreaPostion, new Vector2(Screen.width / 2, Screen.height / 2));

        float radius = checkArea.rectTransform.sizeDelta.x * 0.5f;

        return distanceToCheckArea <= radius; ;
    }

    void CountTime()
    {
        if (startCountTime)
        {
            timeCount += Time.deltaTime;
        }
        else
        {
            timeCount = 0;
        }
    }

    void PointAtMonster()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterLayer))
        {
            if (hit.collider.CompareTag("Monster") || hit.collider.CompareTag("TrueMonster"))
            {
                float distanceToMonster = Vector3.Distance(mainCamera.transform.position, hit.point);

                if (distanceToMonster <= checkRange)    // Can catch the Monster.
                {
                    if (!startCatching)
                    {
                        displayText.GetComponent<TMP_Text>().text = "Can Catch!";
                        displayText.GetComponent<TMP_Text>().color = Color.yellow;
                    }
                    inRange = true;

                    if ( buttonPressed && !startCatching)
                    {
                        startCatching = true;
                        StartCoroutine(StartCatching(hit.collider.gameObject));
                    }
                }
                else    // Too far away from Monster.
                {
                    displayText.GetComponent<TMP_Text>().text = "Too Far!";
                    displayText.GetComponent<TMP_Text>().color = Color.red;
                    inRange = false;
                    startCatching = false;
                }
            }
            else    // Not point at a Monster.
            {
                displayText.GetComponent<TMP_Text>().text = "No Monster!";
                displayText.GetComponent<TMP_Text>().color = Color.blue;
                inRange = false;
                startCatching = false;
            }
        }
        else
        {
            displayText.GetComponent<TMP_Text>().text = "Search Around!";
            displayText.GetComponent<TMP_Text>().color = Color.gray;
            inRange = false;
            startCatching = false; // 重置捕捉状态
        }
    }

    IEnumerator StartCatching(GameObject Monster)
    {
        buttonPressed = false;
        // 显示扫描信息并等待两秒
        displayText.GetComponent<TMP_Text>().text = "Prepare to scan...";
        displayText.GetComponent<TMP_Text>().color= Color.green;
        yield return new WaitForSeconds(2f); // 等待2秒

        // 激活扫描区域
        checkArea.gameObject.SetActive(true);
        startCountTime = true;

        float stayTime = 0f;

        while (startCatching) // 使用 startCatching 来控制循环
        {
            if (IsPointInsideCheckArea(Monster))
            {
                displayText.GetComponent<TMP_Text>().text = $"Scanning... {stayTime:F1}";
                stayTime += Time.deltaTime;

                if (stayTime >= 5f)
                {
                    if (Monster.CompareTag("TrueMonster"))
                    {
                        displayText.GetComponent<TMP_Text>().text = "Monster caught!";
                        checkArea.gameObject.SetActive(false);
                        yield return new WaitForSeconds(2f); // 等待2秒
                                                             // TODO: Add some VFX to destroy the monster
                        Destroy(Monster); // 假设您要销毁怪物对象
                        break; // 成功捕捉，退出循环
                    }
                    else
                    {
                        displayText.GetComponent<TMP_Text>().text = "Fake Monster!";
                        checkArea.gameObject.SetActive(false);
                        yield return new WaitForSeconds(2f);
                        // TODO: Hit Effect
                        OnHit(5);   // Get damage

                        break;
                    }
                }
            }
            else
            {
                stayTime = 0f; // 如果怪物移出了检测区域，重置计时器
                displayText.GetComponent<TMP_Text>().text = "Monster escaped!";
                checkArea.gameObject.SetActive(false);
                // TODO: Hit Effect
                OnHit(5);   // Get damage
                yield return new WaitForSeconds(2f); // 等待2秒


                // 可以在这里添加逻辑，如果怪物逃走了，是否停止捕捉
                startCatching = false;

                break;
            }
            yield return null;

            Debug.Log(stayTime);
        }

        
        startCountTime = false;
        startCatching = false;
    }

    public int GetSan()
    {
        return San;
    }

    void OnHit(int damage)
    {
        San -= damage;

        onHitAction?.Invoke();

        if (San <= 0)
        {
            // Game Over
        }
    }
}
