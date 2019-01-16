using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdDownSlider : MonoBehaviour {

    Slider slider;
    Image fillImage;

    Color newColor;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        fillImage = GameObject.FindGameObjectWithTag(Tags.COUNTDOWN_FILL_TAG).GetComponent<Image>();
    }

    private void Update()
    {
        slider.value = GameManager.Instance.CountDownValue;

        if(slider.value >= slider.maxValue * 0.7)
        {
            fillImage.color = Color.green;
            newColor = Color.green;
        }
        else if(slider.value >= slider.maxValue * 0.3)
        {
            newColor = Color.yellow;
        }
        else
        {
            newColor = Color.red;
        }

        fillImage.color = Color.Lerp(fillImage.color, newColor, 0.05f);

        if (slider.value == 0)
        {
            slider.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        canvasGroup.alpha = 0f;
    }

    IEnumerator FadeIn()
    {
        float time = 1f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
}
