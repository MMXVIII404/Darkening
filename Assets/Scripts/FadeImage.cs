using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade; // ��Ҫ����ΪҪ������ͼ��

    public GameObject startMenu;
    // ���������������ʼ����Ч��
    public void StartFading()
    {
        StartCoroutine(FadeOut(imageToFade, 3f)); // 3���ڵ���
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float counter = 0;

        // ��ȡͼ��ĳ�ʼ alpha ֵ
        Color startColor = image.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0, counter / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
    }
}
