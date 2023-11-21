using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AinOnTarget : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 3f;
    public GameObject displayText;

    bool buttonPressed = false;
    bool inRange = false;
    string currentMonsterName = string.Empty;
    //public Button catchButton;

    void Start()
    {
        // mainCamera = Camera.main;
    }

    void Update()
    {
        Catch();
    }

    private void Catch()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterLayer))
        {
            if (hit.collider.CompareTag("Monster"))
            {

                float distanceToMonster = Vector3.Distance(mainCamera.transform.position, hit.point);
                // Debug.Log(distanceToMonster);
                if (distanceToMonster > checkRange)     // ׼��Ϊ�ʺ�
                {
                    displayText.GetComponent<TMP_Text>().text = "?";
                    displayText.GetComponent<TMP_Text>().color = Color.red;
                    // Debug.Log("Too far!");
                }
                else if (distanceToMonster <= checkRange)    // ׼��Ϊ��׽��
                {
                    displayText.GetComponent<TMP_Text>().text = "Scanning...";
                    inRange = true;
                    displayText.GetComponent<TMP_Text>().color = Color.yellow;

                    if (buttonPressed)
                    {
                        // TODO: Add some VFX to destroy the monster

                        currentMonsterName = hit.collider.gameObject.name;
                        Destroy(hit.collider.gameObject);

                        buttonPressed = false;
                        inRange = false;
                    }

                    // Debug.Log("Can catch!");
                }
                else
                {
                    displayText.GetComponent<TMP_Text>().text = "No Monster";
                    // Debug.Log("Nothing!");
                }
            }
        }
    }
    public void SetCatchStatus(bool status)
    {
        if (inRange)
        {
            buttonPressed = status;
        }
    }
}
