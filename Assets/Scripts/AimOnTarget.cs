using TMPro;
using UnityEngine;

public class AinOnTarget : MonoBehaviour
{
    Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 1f;
    public TextMeshPro displayText;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, monsterLayer))
        {
            if (hit.collider.CompareTag("Monster"))
            {

                float distanceToMonster = Vector3.Distance(mainCamera.transform.position, hit.point);
                if (distanceToMonster > checkRange)     // ׼��Ϊ�ʺ�
                {
                    displayText.text = "?";
                    displayText.color = Color.red;
                    Debug.Log("Too far!");
                }
                else if (distanceToMonster <= checkRange)    // ׼��Ϊ��׽��
                {
                    displayText.text = "Scanning...";
                    displayText.color = Color.yellow;
                    Debug.Log("Can catch!");
                }
                else
                {
                    displayText.text = "";
                    Debug.Log("Nothing!");
                }
            }
        }
    }
}
