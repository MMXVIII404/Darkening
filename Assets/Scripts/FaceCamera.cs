using UnityEngine;

public class FaceCamera : MonoBehaviour
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
            targetDirection.y = 0; // 忽略Y轴的差异，保证旋转只在水平面上

            Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}
