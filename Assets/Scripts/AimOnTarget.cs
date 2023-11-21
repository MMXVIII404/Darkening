using TMPro;
using UnityEngine;

public class AinOnTarget : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 1f;
    public GameObject displayText;
    [SerializeField]
    bool buttonPressed = false;
    string currentCatchMonster = "";
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
                    displayText.GetComponent<TMP_Text>().color = Color.yellow;

                    if (buttonPressed)
                    {
                        // TODO: Add some VFX to destroy the monster
                        currentCatchMonster = hit.collider.gameObject.name;
                        Destroy(hit.collider.gameObject);

                        buttonPressed = false;

                    }

                    // Debug.Log("Can catch!");
                }
                else
                {
                    displayText.GetComponent<TMP_Text>().text = "";
                    // Debug.Log("Nothing!");
                }
            }
        }
    }
    public void SetbuttonPressed(bool status)
    {
        buttonPressed = status;
    }
}
