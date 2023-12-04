using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    string name = string.Empty;
    [SerializeField] MonsterDescription monsterDescription;

    public void SetName(string name) { this.name = name; }
    public string GetName() { return name; }

    public void SetMonster()
    {
        monsterDescription.SetMonsterInformation(this);
    }
}
