using UnityEngine;

public class Fly : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    private AimOnTarget aimOnTarget;

    private Vector3 startPos;

    void Start()
    {
        this.amplitude = Random.Range(0.13f, 0.15f);
        this.frequency = Random.Range(0.2f, 0.3f);
        aimOnTarget = GameObject.Find("GameLogic").GetComponent<AimOnTarget>();
        startPos = transform.position;
    }

    void Update()
    {
          if(!aimOnTarget.isAttacking){
             float tempPos = amplitude * Mathf.Sin(Time.time * Mathf.PI * frequency);
             transform.position = startPos + Vector3.up * tempPos;
          }
    }
}
