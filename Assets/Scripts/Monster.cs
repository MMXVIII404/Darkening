using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject slots; // �� GameObject

    public void AssignNameToSlot()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject child = slots.transform.GetChild(i).gameObject;
            MonsterSlot script = child.GetComponent<MonsterSlot>();

            if (script != null && !script.GetContainStatus())
            {
                // ��� GetContainStatus ���� false�����������Ʋ����� SetContainStatus
                script.SetName(this.name);
                script.SetContainStatus();
                break; // ��ֵ�ɹ����˳�ѭ��
            }
        }
    }
}
