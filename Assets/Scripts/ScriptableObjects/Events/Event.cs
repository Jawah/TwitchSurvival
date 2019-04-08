using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

[System.Serializable]
public class Event : ScriptableObject
{
    public float eventLength;
    public List<string> possibleAnswers = new List<string>();

    void OnEnable()
    {
        eventLength = PlayerPrefs.GetInt("VotingSpeed");
    }
    
    public virtual void Instantiate()
    {
        GameManager.Instance.CountDownValue = eventLength;
        GameManager.Instance.interfaceHandler.countDownSlider.maxValue = eventLength;
        GameManager.Instance.interfaceHandler.EnableCountDownSlider();

        for (int i = 0; i < possibleAnswers.Count; i++)
        {
            GameObject tempAnswer = GameManager.Instance.interfaceHandler.answerPrefab;
            tempAnswer.GetComponent<TextMeshProUGUI>().text = possibleAnswers[i];
            Instantiate(tempAnswer, GameManager.Instance.interfaceHandler.answersPanel.transform);

            GameObject tempSlider = GameManager.Instance.interfaceHandler.sliderPrefab;
            tempSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = possibleAnswers[i];
            Instantiate(tempSlider, GameManager.Instance.interfaceHandler.resultPanel.transform);
        }

    }

    public virtual void Execute(List<List<string>> dividedList, CharacterManager characterV) { Debug.LogWarning("No override Execute function declared!"); }

    public virtual void Execute(List<List<string>> dividedList) { Debug.LogWarning("No override Execute function declared!"); }
    
    public virtual void Execute() {Debug.LogWarning("No override Execute function declared"); }
}