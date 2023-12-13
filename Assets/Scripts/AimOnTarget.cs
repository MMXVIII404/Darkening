using System;
using System.Collections;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AimOnTarget : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 2f;
    public GameObject displayText;

    public Image checkArea;
    public Slider checkAreaSlider;

    public event Action onHitAction;
    public event Action gameOverAction;
    public SimplePlayer sanEffect;
    public Button beginButton;
    public Battery battery;
    private AudioSource audioSource;
    public int hitValue = 20;
    public GameObject gameoverPanel;

    bool buttonPressed = false;
    bool inRange = false;
    string currentMonsterName = string.Empty;

    bool startCountTime = false;
    bool catched = false;
    bool startCatching = false;
    int San = 100;
    GameObject currentMonster;
    Vector2 checkAreaPosition;
    //add by ljh start
    private Animator _animator;
    private AudioSource idleAudioSource;
    private AudioSource attackAudioSource;
    public bool isAttacking = false;
    public PlaneDetectionManager planeDetectionManager;
    public GameObject LeftRedImage;
    public GameObject RightRedImage;

    //add by ljh end

    //public Button catchButton;
    //111
    void Start()
    {
        // mainCamera = Camera.main;
    }

    void Update()
    {
        //add by ljh start
        if (!isAttacking)
        {
            if (currentMonster != null  && currentMonster.tag == "TrueMonster")
            {
                currentMonster.transform.GetComponent<AudioSource>().mute = false;
            }
            

        }
        if (currentMonster != null  && currentMonster.tag == "Monster")
        {
            currentMonster.transform.GetComponent<AudioSource>().mute = false;
        }
        //add by ljh end
        //Catch();
        PointAtMonster();
        FollowMonster();
        // Debug.Log(buttonPressed);
    }


    public void SetCatchStatus()
    {
        if (inRange)
        {
            buttonPressed = true;
        }
        else
        {
            battery.UseBattery(0.05f);
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

                    if (buttonPressed && !startCatching)
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

    void FollowMonster()
    {
        if (currentMonster != null)
        {
            checkAreaPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, currentMonster.transform.position);    // Monster postion on screen
            checkArea.GetComponent<RectTransform>().position = checkAreaPosition;   // Check area follow the Monster position on screen
        }
    }
    private bool IsPointInsideCheckArea(GameObject Monster)
    {
        float distanceToCheckArea = Vector2.Distance(checkAreaPosition, new Vector2(Screen.width / 2, Screen.height / 2));

        float radius = checkArea.rectTransform.sizeDelta.x * 0.5f + 80f;
        return distanceToCheckArea <= radius;
    }

    IEnumerator StartCatching(GameObject Monster)
    {
        currentMonster = Monster;
        buttonPressed = false;
        // 显示扫描信息并等待两秒
        displayText.GetComponent<TMP_Text>().text = "Prepare to scan...";
        displayText.GetComponent<TMP_Text>().color = Color.green;
        checkArea.gameObject.SetActive(true);
        checkAreaSlider.gameObject.SetActive(true);
        checkAreaSlider.value = 0f;
        yield return new WaitForSeconds(0.5f); // 等待2秒

        // 激活扫描区域

        startCountTime = true;

        float stayTime = 0f;

        while (startCatching) // 使用 startCatching 来控制循环
        {
            if (IsPointInsideCheckArea(currentMonster))
            {
                displayText.GetComponent<TMP_Text>().text = $"Scanning... {stayTime:F1}";
                stayTime += Time.deltaTime;
                checkAreaSlider.value = stayTime / 5f;
                battery.UseBattery(Time.deltaTime * 0.02f);
                if (stayTime >= 5f)
                {
                    if (currentMonster.CompareTag("TrueMonster"))
                    {
                        displayText.GetComponent<TMP_Text>().text = "Monster caught!";
                        checkArea.gameObject.SetActive(false);
                        Monster.GetComponent<Monster>().AssignMonsterToSlot();
                        yield return new WaitForSeconds(1f); 
                        //beginButton.gameObject.SetActive(true);
                        // TODO: Add some VFX to destroy the monster
                        //Destroy(Monster); // 假设您要销毁怪物对象

                        planeDetectionManager.DestroyAllMonster();

                        break; // 成功捕捉，退出循环
                    }
                    else
                    {
                        //add by ljh start
                        StartCoroutine(AttackAndChangePosition());
                        //add by ljh end
                        displayText.GetComponent<TMP_Text>().text = "Fake Monster!";
                        checkArea.gameObject.SetActive(false);
                        OnHit(hitValue);   // Get damage
                        //Destroy(Monster);
                        yield return new WaitForSeconds(1f);
                        // TODO: Hit Effect

                        break;
                    }
                }
            }
            else
            {
                //add by ljh start
                StartCoroutine(AttackAndChangePosition());
                //add by ljh end
                stayTime = 0f; // 如果怪物移出了检测区域，重置计时器
                displayText.GetComponent<TMP_Text>().text = "Monster escaped!";
                checkArea.gameObject.SetActive(false);
                // TODO: Hit Effect
                OnHit(hitValue);   // Get damage
                yield return new WaitForSeconds(1f); // 等待2秒


                // 可以在这里添加逻辑，如果怪物逃走了，是否停止捕捉
                startCatching = false;

                break;
            }
            yield return null;
        }
        checkArea.gameObject.SetActive(false);


        startCountTime = false;
        startCatching = false;
    }
    //add by ljh start
    private IEnumerator AttackAndChangePosition()
    {
        isAttacking = true;
        _animator = currentMonster.GetComponent<Animator>();
        Transform transformBackup = currentMonster.transform;
        currentMonster.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.3f;
        _animator.SetTrigger("Eye_Attack");

        if (currentMonster.tag == "TrueMonster")
        {
            idleAudioSource = currentMonster.transform.GetComponent<AudioSource>();
            idleAudioSource.mute = true;
        }
        
        attackAudioSource = currentMonster.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();
        attackAudioSource.PlayOneShot(attackAudioSource.clip);
        
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        currentMonster.transform.position = new Vector3(-transformBackup.position.x, transformBackup.position.y, transformBackup.position.z);
        planeDetectionManager.trueMonsterNumber = UnityEngine.Random.Range(2, planeDetectionManager.maxMonsters - 1);
        StartCoroutine(planeDetectionManager.SwitchTrueMonster());
        // StopCoroutine(AttackAndChangePosition());
    }
    //add by ljh end

    public int GetSan()
    {
        return San;
    }

    void OnHit(int damage)
    {
        San -= damage;
        sanEffect.Damage(damage);
        onHitAction?.Invoke();

        if (San <= 0)
        {
            gameoverPanel.SetActive(true);
            RightRedImage.SetActive(false);
            LeftRedImage.SetActive(false);
        }
    }
}
