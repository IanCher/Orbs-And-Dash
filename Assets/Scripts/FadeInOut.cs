using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeInOut : MonoBehaviour
{
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] bool isFadingIn = false;
    [SerializeField] bool isFadingOut = false;

    Image fadeImage;

    void Start()
    {
        fadeImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFadingOut && !isFadingIn) return;

        if (isFadingIn)
        {
            Color currentColor = fadeImage.color;
            if (currentColor.a == 0)
            {
                isFadingIn = false;
                return;
            }

            fadeImage.color = new Color(
                currentColor.r,
                currentColor.g,
                currentColor.b,
                currentColor.a - 1 / fadeInTime * Time.deltaTime
            );
        }

        if (isFadingOut)
        {
            Color currentColor = fadeImage.color;
            if (currentColor.a == 1)
            {
                isFadingOut = false;
                return;
            }

            fadeImage.color = new Color(
                currentColor.r,
                currentColor.g,
                currentColor.b,
                currentColor.a + 1 / fadeInTime * Time.deltaTime
            );
        }
    }

    public void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
    }

    public void StartFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }
    
    public float GetFadeOutTime()
    {
        return fadeOutTime;
    }
}
