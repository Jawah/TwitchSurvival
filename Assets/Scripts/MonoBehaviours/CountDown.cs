using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour {

    private float counterValue;
    private int counterValueInt;

    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
        counterValue = float.Parse(counterText.text);
    }

    void Update ()
    {
        counterValue -= Time.deltaTime;

        counterValueInt = (int)counterValue;
        Debug.Log(counterValueInt);
        counterText.text = counterValueInt.ToString();
    }
}
