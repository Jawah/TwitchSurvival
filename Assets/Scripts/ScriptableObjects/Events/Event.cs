using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Event : ScriptableObject
{
    public float m_EventLength;

    [TextArea]
    public string m_EventDescription;

    public List<string> m_PossibleAnswers = new List<string>();

    public virtual void Instantiate()
    {
        for (int i = 0; i < m_PossibleAnswers.Count; i++)
        {
            GameObject tempAnswer = GameManager.Instance.m_InterfaceHandler.m_AnswerPrefab;
            tempAnswer.GetComponent<TextMeshProUGUI>().text = m_PossibleAnswers[i];
            Instantiate(tempAnswer, GameManager.Instance.m_InterfaceHandler.m_AnswersPanel.transform);

            GameObject tempSlider = GameManager.Instance.m_InterfaceHandler.m_SliderPrefab;
            tempSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = m_PossibleAnswers[i];
            Instantiate(tempSlider, GameManager.Instance.m_InterfaceHandler.m_ResultPanel.transform);
        }

    }

    public virtual void Execute(List<List<string>> dividedList, CharacterManager characterV) { Debug.LogWarning("No override Execute function declared!"); }

    public virtual void Execute(List<List<string>> dividedList) { Debug.LogWarning("No override Execute function declared!"); }
}