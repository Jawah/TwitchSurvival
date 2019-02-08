using System.Collections;
using UnityEngine;
using TMPro;

public class TypingText : MonoBehaviour
{
    private TextMeshProUGUI target;
    private string currentText;
    private string newText;
    private float timer;
    private int i;

    [SerializeField]
    private AudioSource typingSound;
    
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
        
        if(!typingSound.isPlaying) typingSound.Play();
        
        timer += Time.unscaledDeltaTime * Time.timeScale;

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
            typingSound.Stop();
        }
        
    }
    
    public IEnumerator TypeRoutine(string txt)
    {
        target.text = null;
        newText = txt;
        yield return new WaitUntil(() => target.text == txt);
        yield return new WaitForSeconds(3f);
    }

    public void Type(string txt)
    {
        target.text = null;
        newText = txt;
    }
}