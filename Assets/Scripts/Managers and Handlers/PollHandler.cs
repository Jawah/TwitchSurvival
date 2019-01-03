using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollHandler : MonoBehaviour {

    [Header("Hallo Konni")]
    
    [HideInInspector] public List<string> votersList = new List<string>();
    [HideInInspector] public List<string> pollAnswers = new List<string>();
    [HideInInspector] public List<List<string>> listOfValidAnswersDivided = new List<List<string>>();
    [HideInInspector] public bool gatherVotes;
    [HideInInspector] public List<string> currentPossibleAnswers = new List<string>();
    [HideInInspector] public int numberOfValidAnswers;

    public IEnumerator DoPoll(Event eventV, CharacterManager characterV)
    {
        GameManager.Instance.eventHandler.m_CurrentEvent = eventV;
        GameManager.Instance.characterHandler.currentCharacter = characterV;
        currentPossibleAnswers = eventV.possibleAnswers;

        eventV.Instantiate();
        gatherVotes = true;

        yield return new WaitForSeconds(eventV.eventLength);

        CalculateAnswers();
        eventV.Execute(listOfValidAnswersDivided, characterV);
        gatherVotes = false;

        ResetForNewEvent();
    }

    public IEnumerator DoPoll(Event eventV)
    {
        GameManager.Instance.eventHandler.m_CurrentEvent = eventV;
        currentPossibleAnswers = eventV.possibleAnswers;

        eventV.Instantiate();
        gatherVotes = true;

        yield return new WaitForSeconds(eventV.eventLength);

        CalculateAnswers();
        eventV.Execute(GameManager.Instance.pollHandler.listOfValidAnswersDivided);
        gatherVotes = false;

        ResetForNewEvent();
    }

    public void CalculateAnswers()
    {
        listOfValidAnswersDivided.Clear();

        for (int i = 0; i < GameManager.Instance.eventHandler.m_CurrentEvent.possibleAnswers.Count; i++)
        {
            listOfValidAnswersDivided.Add(new List<string>());
        }

        foreach (var answer in pollAnswers)
        {
            for (int k = 0; k < GameManager.Instance.eventHandler.m_CurrentEvent.possibleAnswers.Count; k++)
            {
                if (answer.ToLower() == GameManager.Instance.eventHandler.m_CurrentEvent.possibleAnswers[k].ToLower())
                {
                    listOfValidAnswersDivided[k].Add(answer.ToLower());
                    numberOfValidAnswers++;
                }
            }
        }

        if (pollAnswers.Count != 0)
        {
            for (int l = 0; l < listOfValidAnswersDivided.Count; l++)
            {
                float newValue = listOfValidAnswersDivided[l].Count / (float)pollAnswers.Count;
                GameManager.Instance.interfaceHandler.resultPanel.transform.GetChild(l).GetComponentInChildren<Slider>().value = newValue;
            }
        }
    }

    private void ResetForNewEvent()
    {
        foreach (Transform child in GameManager.Instance.interfaceHandler.answersPanel.transform)
        {
            Destroy(child.gameObject);
        }

        GameManager.Instance.interfaceHandler.questionText.text = null;

        foreach (Transform child in GameManager.Instance.interfaceHandler.resultPanel.transform)
        {
            Destroy(child.gameObject);
        }

        GameManager.Instance.characterHandler.currentCharacter = null;
        GameManager.Instance.eventHandler.m_CurrentEvent = null;

        votersList.Clear();
        pollAnswers.Clear();
        listOfValidAnswersDivided.Clear();
        GameManager.Instance.informationManager.Reset();
    }
}