using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade; // ��Ҫ����ΪҪ������ͼ��
    public Image startButtonImage;
    public Image aboutButtonImage;
    public Image exitButtonImage;
    public Image logo1Image;
    public Image logo2Image;
    public GameObject startMenu;
    public TextMeshProUGUI header;
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
            startButtonImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            aboutButtonImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            exitButtonImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            logo1Image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            logo2Image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            Color color = header.color;
            color.a = alpha;
            header.color = color;

            yield return null;
        }
        startMenu.SetActive(false);
    }
}
