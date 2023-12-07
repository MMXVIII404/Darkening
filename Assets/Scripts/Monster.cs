using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject slots; // 父 GameObject

    private void Start()
    {
        slots = GameObject.Find("Album").transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
    }
    public void AssignMonsterToSlot()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject child = slots.transform.GetChild(i).gameObject;
            MonsterSlot slot = child.GetComponent<MonsterSlot>();

            if (slot != null && !slot.GetContainStatus())
            {
                // 如果 GetContainStatus 返回 false，则设置名称并调用 SetContainStatus
                slot.SetName(this.name);
                slot.SetContainStatus();
                switch (gameObject.name)
                {
                    case "Monster 1":
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster1");
                        break;
                    case "Monster 2":
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster2");
                        break;
                    case "Monster 3":
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster3");
                        break;
                }
                break; // 赋值成功后退出循环
            }
        }
    }
}
