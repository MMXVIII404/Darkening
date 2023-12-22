using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateSan : MonoBehaviour
{
    public AimOnTarget gameLogic;
    void Start()
    {
        gameLogic.onHitAction += RefreshSan;
    }

    void Update()
    {
    }

    void RefreshSan()
    {
        this.GetComponent<TMP_Text>().text = gameLogic.GetSan().ToString();
    }
}
