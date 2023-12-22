using UnityEngine;

public class FaceCamera2 : MonoBehaviour
{
    public GameObject target; // 目标GameObject的公共引用

    private void Start() {
        if(target == null){
            target = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    void Update()
    {
        if (target != null)
        {
             Vector3 targetDirection = target.transform.position - transform.position;
            //targetDirection.y = 0; // Ignore y axis

            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
        }
    }
}
