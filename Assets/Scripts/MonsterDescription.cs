using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDescription : MonoBehaviour
{
    public Image monsterImage;
    //public Sprite MonsterSprite1;
    //public Sprite sprite2;
    //public Sprite sprite3;

    Dictionary<string, string> monsterInformation = new Dictionary<string, String>()
        {
            { "Monster 1", " Monster 1 description" },
            { "Monster 2", " Monster 2 description" },
            { "Monster 3", " Monster 3 description" }
        };

    public TextMeshProUGUI monsterName;
    public TextMeshProUGUI monsterDescription;

    public void SetMonsterInformation(MonsterSlot monster)
    {
        string name = monster.GetName();
        if (monsterInformation.ContainsKey(name))
        {
            monsterName.text = name;
            monsterDescription.text = monsterInformation[name];
        }
        else
        {
            monsterName.text = "Unknown Monster";
            monsterDescription.text = "Search around!";
        }

        UpdatePicture();
    }

    private void UpdatePicture()
    {
        switch (monsterName.text)
        {
            case "Monster 1":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster1");
                break;
            case "Monster 2":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster2");
                break;
            case "Monster 3":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster3");
                break;
        }
    }
}
