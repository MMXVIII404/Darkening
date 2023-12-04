using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDescription : MonoBehaviour
{
    Dictionary<string, string> monsterInformation = new Dictionary<string, String>();
    public TextMeshProUGUI monsterName;
    public TextMeshProUGUI monsterDescription;

    private void Start()
    {
        monsterInformation.Add("Monster 1", " Monster 1 description");
        monsterInformation.Add("Monster 2", " Monster 2 description");
        monsterInformation.Add("Monster 3", " Monster 3 description");
    }

    public void SetMonsterInformation(Monster monster)
    {
        string name = monster.GetName();
        monsterName.text = name;
        if (monsterInformation.ContainsKey(name))
        {
            monsterDescription.text = monsterInformation[name];
        }
    }
}
