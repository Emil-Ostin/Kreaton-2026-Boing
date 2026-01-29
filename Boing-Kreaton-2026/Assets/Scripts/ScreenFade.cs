using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image myImage;


    void Awake()
    {
        myImage = GetComponent<Image>();
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0);
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 1);
        Color targetColor = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0);

        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 0);
        Color targetColor = new Color(myImage.color.r, myImage.color.g, myImage.color.b, 1);

        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    private IEnumerator FadeCoroutine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            myImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}
