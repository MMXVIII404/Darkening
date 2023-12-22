using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    public Image imageToFade;
    public Image startButtonImage;
    public Image logo1Image;
    public Image logo2Image;
    public GameObject startMenu;
    public TextMeshProUGUI header;
    public TextMeshProUGUI startText;
    public FadeWarning fadeWarning;

    public void StartFading()
    {
        StartCoroutine(FadeOut(imageToFade, 3f));
    }

    private IEnumerator FadeOut(Image image, float duration)
    {
        float counter = 0;

        Color startColor = image.color;
        Color color = header.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0, counter / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            startButtonImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            logo1Image.color = new Color(logo1Image.color.r, logo1Image.color.g, logo1Image.color.b, alpha);
            logo2Image.color = new Color(logo2Image.color.r, logo2Image.color.g, logo2Image.color.b, alpha);
            color.a = alpha;
            header.color = color;
            color = startText.color;
            color.a = alpha;
            startText.color = color;
            yield return null;
        }
        fadeWarning.StartShowing();
        startMenu.SetActive(false);
    }
}
