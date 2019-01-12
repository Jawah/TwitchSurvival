using UnityEngine;
using TMPro;

public class TypingText : MonoBehaviour
{
    private TextMeshProUGUI target;
    private string currentText;
    private string newText;
    private float timer;
    private int i;
    
    public float timeInSeconds;

    private void Awake()
    {
        target = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        currentText = target.text;
        newText = target.text;
    }

    private void Update()
    {
        if (currentText == newText) return;
        timer += Time.deltaTime;

        if (timer >= timeInSeconds && i <= newText.Length)
        {
            target.text = newText.Substring(0, i);
            timer = 0;
            i++;
        }

        if (i > newText.Length)
        {
            i = 0;
            currentText = newText;
        }
    }

    public void Type(string txt)
    {
        newText = txt;
    }
}