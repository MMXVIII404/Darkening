using UnityEngine;

public class Fly : MonoBehaviour
{
    public float amplitude = 0.5f; // 浮动的幅度
    public float frequency = 1f; // 浮动的频率
    private AimOnTarget aimOnTarget;

    private Vector3 startPos;

    void Start()
    {
        aimOnTarget = GameObject.Find("GameLogic").GetComponent<AimOnTarget>();
        startPos = transform.position;
    }

    void Update()
    {
          if(!aimOnTarget.isAttacking){
             float tempPos = amplitude * Mathf.Sin(Time.time * Mathf.PI * frequency);
             transform.position = startPos + Vector3.up * tempPos;
          }
        // 计算上下浮动的新位置
       
    }
}
