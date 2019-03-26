using System.Collections;
using UnityEngine;

public class CanvasFade : MonoBehaviour {
    
    private CanvasGroup _canvasGroup;

    private void OnEnable() {
        _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 1;

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
        yield return new WaitForSeconds(4f);
    }

    private IEnumerator DoFade()
    {
        while(_canvasGroup.alpha > 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
    }
}
