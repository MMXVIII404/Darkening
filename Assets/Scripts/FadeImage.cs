using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade; // 需要设置为要淡出的图像
    public Image startButtonImage;
    public Image aboutButtonImage;
    public Image exitButtonImage;
    public Image logo1Image;
    public Image logo2Image;
    public GameObject startMenu;
    public TextMeshProUGUI header;
    // 调用这个方法来开始淡出效果
    public void StartFading()
    {
        StartCoroutine(FadeOut(imageToFade, 3f)); // 3秒内淡出
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float counter = 0;

        // 获取图像的初始 alpha 值
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
