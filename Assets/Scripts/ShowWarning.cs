using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeWarning : MonoBehaviour
{
    public Image warning;
    public GameObject warningSection;
    public GameObject warningBack;
    public bool fadingDone = false;
    float transitionTime = 1.5f;
    public void StartShowing()
    {
        StartCoroutine(ShowImage(warning, transitionTime));
    }

    private IEnumerator FadeImage(Image image, float duration)
    {
        float counter = 0;
        Color startColor = image.color;
        Color warningStartColor = warningSection.GetComponent<Image>().color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0, counter / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            warningSection.GetComponent<Image>().color = new Color(warningStartColor.r, warningStartColor.g, warningStartColor.b, alpha);
            yield return null;
        }
        warningSection.SetActive(false);
        warningBack.SetActive(false);
        fadingDone = true;
    }

    private IEnumerator ShowImage(Image image, float duration)
    {
        float counter = 0;

        Color startColor = image.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / duration);
            image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        StartCoroutine(WaitTwoSeconds());
    }

    private IEnumerator WaitTwoSeconds()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeImage(warning, transitionTime));
    }
}
