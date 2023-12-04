using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject slots; // �� GameObject

    public void AssignMonsterToSlot()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject child = slots.transform.GetChild(i).gameObject;
            MonsterSlot slot = child.GetComponent<MonsterSlot>();

            if (slot != null && !slot.GetContainStatus())
            {
                // ��� GetContainStatus ���� false�����������Ʋ����� SetContainStatus
                slot.SetName(this.name);
                slot.SetContainStatus();
                switch (gameObject.name)
                {
                    case "Monster 1":
                        child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster1");
                        break;
                    case "Monster 2":
                        child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster2");
                        break;
                    case "Monster 3":
                        child.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster3");
                        break;
                }
                break; // ��ֵ�ɹ����˳�ѭ��
            }
        }
    }
}
