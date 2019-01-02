using System.Collections;
using UnityEngine;

public class CanvasFade : MonoBehaviour {
    private CanvasGroup canvasGroup;

    private void OnEnable() {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1;

        Fade();
	}

    public void Fade()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        yield return StartCoroutine(Wait());
        yield return StartCoroutine(DoFade());

        gameObject.SetActive(false);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator DoFade()
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
    }
}
