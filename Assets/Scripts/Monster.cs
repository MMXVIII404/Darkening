using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject slots; // 父 GameObject

    public void AssignNameToSlot()
    {
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            GameObject child = slots.transform.GetChild(i).gameObject;
            MonsterSlot script = child.GetComponent<MonsterSlot>();

            if (script != null && !script.GetContainStatus())
            {
                // 如果 GetContainStatus 返回 false，则设置名称并调用 SetContainStatus
                script.SetName(this.name);
                script.SetContainStatus();
                break; // 赋值成功后退出循环
            }
        }
    }
}
