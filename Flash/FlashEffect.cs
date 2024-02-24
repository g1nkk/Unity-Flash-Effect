using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] 
    private float defaultDuration = 1;
    
    [SerializeField] 
    private float hideSpeedMultiplier = .5f;

    [SerializeField]
    private GameObject canvasPrefab;

    [SerializeField]
    private AudioSource flashAudioSource;

    public void Flash()
    {
        StartCoroutine(FlashCoroutine(defaultDuration));
    }

    public void Flash(float durationSeconds)
    {
        StartCoroutine(FlashCoroutine(durationSeconds));
    }

    public void FlashDelayed(float delaySeconds)
    {
        StartCoroutine(FlashDelayedCoroutine(delaySeconds, defaultDuration));
    }
    
    public void FlashDelayed(float delaySeconds, float durationSeconds)
    {
        StartCoroutine(FlashDelayedCoroutine(delaySeconds, durationSeconds));
    }
    
    IEnumerator FlashDelayedCoroutine(float delaySeconds, float durationSeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        StartCoroutine(FlashCoroutine(durationSeconds));
    }

    IEnumerator FlashCoroutine(float durationSeconds)
    {
        flashAudioSource.Play();
        
        yield return new WaitForEndOfFrame();
        
        // create canvas and set flash screenshot
        var canvas = Instantiate(canvasPrefab);

        var screenshotImage = canvas.transform.GetChild(0).GetComponent<Image>();
        screenshotImage.sprite = GetScreenshotSprite();

        // fade white background
        yield return new WaitForSeconds(durationSeconds);
        StartCoroutine(HideCanvasGroup(canvas.transform.GetChild(1).GetComponent<CanvasGroup>()));
        
        // fade screenshot
        yield return new WaitForSeconds(.35f);
        yield return StartCoroutine(HideCanvasGroup(canvas.transform.GetChild(0).GetComponent<CanvasGroup>()));
        
        Destroy(canvas);
    }

    Sprite GetScreenshotSprite()
    {
        var screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        return Sprite.Create(screenshotTexture, new Rect( 0.0f, 0.0f, screenshotTexture.width, screenshotTexture.height ), Vector2.zero);
    }

    IEnumerator HideCanvasGroup(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * hideSpeedMultiplier;
            yield return null;
        }
    }
}