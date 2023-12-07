using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public GameObject slots; // �� GameObject
    public int MonsterType = 1;

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
                // ��� GetContainStatus ���� false�����������Ʋ����� SetContainStatus
                slot.SetName(this.name);
                slot.SetContainStatus();
                switch (MonsterType)
                {
                    case 1:
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster1");
                        break;
                    case 2:
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster2");
                        break;
                    case 3:
                        child.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Monster3");
                        break;
                }
                break; // ��ֵ�ɹ����˳�ѭ��
            }
        }
    }

    public void SetMonsterTyoe(int monsterType)
    {
        this.monsterType = monsterType;
    }
}
