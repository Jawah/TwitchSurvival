using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFadeOut : MonoBehaviour {
    
	void Start () {
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
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        //yield return null;
    }
}
