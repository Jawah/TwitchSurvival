using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFade : MonoBehaviour {

    CanvasGroup canvasGroup;

    void OnEnable() {
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
    }

    IEnumerator DoFade()
    {
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        //yield return null;
    }
}
