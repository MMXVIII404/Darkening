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
            { "Grettos", "Attack intensity: Low\n" +
            "Level: I\n" +
            "Appearance: There are five petal shaped tentacles on the eye. Underneath are two bones made of claws, which are used to escape if necessary\n" +
            "Behavior: Petal-like tentacles close when startled. When angry, it will jump directly on the intruder's head and attack them"},
            { "Monster 2", " Monster 2 description" },
            { "Monster 3", " Monster 3 description" }
        };

    public TextMeshProUGUI monsterName;
    public TextMeshProUGUI monsterDescription;

    public void SetMonsterInformation(MonsterSlot monsterSlot)
    {
        string name = monsterSlot.GetName();
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
            case "Grettos":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Grettos");
                break;
            case "Monster 2":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster2");
                break;
            case "Monster 3":
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Monster3");
                break;
            default:
                monsterImage.sprite = Resources.Load<Sprite>("Sprites/Unknown");
                break;
        }
    }
}
