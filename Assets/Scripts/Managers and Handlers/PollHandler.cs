using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollHandler : MonoBehaviour {

    public List<string> m_VotersList = new List<string>();

    public List<string> m_PollAnswers = new List<string>();
    public List<List<string>> m_ListOfValidAnswersDivided = new List<List<string>>();

    public bool m_GatherVotes;

    public List<string> m_CurrentPossibleAnswers = new List<string>();

    public int numberOfValidAnswers;

    public IEnumerator DoPoll(Event eventV, CharacterManager characterV)
    {
        GameManager.Instance.m_EventHandler.m_CurrentEvent = eventV;
        GameManager.Instance.m_CharacterHandler.m_CurrentCharacter = characterV;
        m_CurrentPossibleAnswers = eventV.m_PossibleAnswers;

        eventV.Instantiate();
        m_GatherVotes = true;

        yield return new WaitForSeconds(eventV.m_EventLength);

        CalculateAnswers();
        eventV.Execute(m_ListOfValidAnswersDivided, characterV);
        m_GatherVotes = false;

        ResetForNewEvent();
    }

    public IEnumerator DoPoll(Event eventV)
    {
        GameManager.Instance.m_EventHandler.m_CurrentEvent = eventV;
        m_CurrentPossibleAnswers = eventV.m_PossibleAnswers;

        eventV.Instantiate();
        m_GatherVotes = true;

        yield return new WaitForSeconds(eventV.m_EventLength);

        CalculateAnswers();
        eventV.Execute(GameManager.Instance.m_PollHandler.m_ListOfValidAnswersDivided);
        m_GatherVotes = false;

        ResetForNewEvent();
    }

    public void CalculateAnswers()
    {
        m_ListOfValidAnswersDivided.Clear();

        for (int i = 0; i < GameManager.Instance.m_EventHandler.m_CurrentEvent.m_PossibleAnswers.Count; i++)
        {
            m_ListOfValidAnswersDivided.Add(new List<string>());
        }

        foreach (var answer in m_PollAnswers)
        {
            for (int k = 0; k < GameManager.Instance.m_EventHandler.m_CurrentEvent.m_PossibleAnswers.Count; k++)
            {
                if (answer.ToLower() == GameManager.Instance.m_EventHandler.m_CurrentEvent.m_PossibleAnswers[k].ToLower())
                {
                    m_ListOfValidAnswersDivided[k].Add(answer.ToLower());
                    numberOfValidAnswers++;
                }
            }
        }

        if (m_PollAnswers.Count != 0)
        {
            for (int l = 0; l < m_ListOfValidAnswersDivided.Count; l++)
            {
                float newValue = m_ListOfValidAnswersDivided[l].Count / (float)m_PollAnswers.Count;
                GameManager.Instance.m_InterfaceHandler.m_ResultPanel.transform.GetChild(l).GetComponentInChildren<Slider>().value = newValue;
            }
        }
    }

    private void ResetForNewEvent()
    {
        foreach (Transform child in GameManager.Instance.m_InterfaceHandler.m_AnswersPanel.transform)
        {
            Destroy(child.gameObject);
        }

        GameManager.Instance.m_InterfaceHandler.m_QuestionText.text = null;

        foreach (Transform child in GameManager.Instance.m_InterfaceHandler.m_ResultPanel.transform)
        {
            Destroy(child.gameObject);
        }

        GameManager.Instance.m_CharacterHandler.m_CurrentCharacter = null;
        GameManager.Instance.m_EventHandler.m_CurrentEvent = null;

        m_VotersList.Clear();
        m_PollAnswers.Clear();
        m_ListOfValidAnswersDivided.Clear();
        GameManager.Instance.m_InformationManager.Reset();
    }
}