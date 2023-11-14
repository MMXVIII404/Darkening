using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    Camera mainCamera;
    public LayerMask monsterLayer;
    public float maxRaycastDistance = 100f;
    public float checkRange = 1f;

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
                    Debug.Log("Too far!");
                }
                else    // ׼��Ϊ��׽��
                {
                    Debug.Log("Can catch!");
                }
            }
        }
    }
}
