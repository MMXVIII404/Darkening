using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSlot : MonoBehaviour
{
    string monsterName = string.Empty;
    [SerializeField] MonsterDescription monsterDescription;
    bool contain = false;
    public void SetName(string name) { monsterName = name; }
    public string GetName() { return monsterName; }

    public void SetMonster() { monsterDescription.SetMonsterInformation(this); }

    public void SetContainStatus() { contain = true; }
    public bool GetContainStatus() { return contain; }
}
