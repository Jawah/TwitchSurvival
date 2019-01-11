using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

[System.Serializable]
public class Event : ScriptableObject
{
    public float eventLength;
    public List<string> possibleAnswers = new List<string>();

    public virtual void Instantiate()
    {
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