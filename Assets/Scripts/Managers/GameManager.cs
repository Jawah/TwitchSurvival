﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int m_Day;
    public int m_Temperature;

    public int m_FoodValue;
    public int m_FirewoodValue;
    public int m_MedPackValue;

    public float m_InformationScreenLength;
    public float m_DayStartingLength;
    public float m_DayPlayingLength;
    public float m_DayEndingLength;

    public TextMeshProUGUI m_DayText;
    public TextMeshProUGUI m_TemperatureText;
    public TextMeshProUGUI m_CountDownText;
    public TextMeshProUGUI m_FoodText;
    public TextMeshProUGUI m_FirewoodText;
    public TextMeshProUGUI m_MedPackText;

    public GameObject m_CharacterCardPanel;
    public GameObject m_CharacterCardPrefab;
    public List<Character> m_AllCharacters = new List<Character>();
    public List<CharacterManager> m_ActiveCharacters = new List<CharacterManager>();

    public GameObject m_ItemPanel;
    public GameObject m_ItemPrefab;
    public List<Item> m_AllItems = new List<Item>();
    public List<ItemManager> m_ActiveItems = new List<ItemManager>();

    public GameObject m_InformationPanel;
    public List<InformationEvent> m_InformationEvents = new List<InformationEvent>();

    public List<Event> m_AllEvents = new List<Event>();

    private List<string> m_PollAnswers = new List<string>();
    private List<List<string>> m_ListOfValidAnswersDivided = new List<List<string>>();

    private float m_CountDownValue;
    private int m_CountDownValueInt;


    private float m_FireWoodStrength = 2f;

    private bool m_GatherVotes = false;

    int numberOfValidAnswers;

    //private enum EventEnum { Food, MedPack };
    //private EventEnum m_EventEnum;

    private WaitForSeconds m_WaitForInformationScreen;
    private WaitForSeconds m_WaitForDayStarting;
    private WaitForSeconds m_WaitForDayPlaying;
    private WaitForSeconds m_WaitForDayEnding;

    private void Awake()
    {
        m_WaitForInformationScreen = new WaitForSeconds(m_InformationScreenLength);
        m_WaitForDayStarting = new WaitForSeconds(m_DayStartingLength);
        m_WaitForDayPlaying = new WaitForSeconds(m_DayPlayingLength);
        m_WaitForDayEnding = new WaitForSeconds(m_DayEndingLength);
    }

    void Start()
    {
        SetDay();
        SetTemperature();
        SetRessources();
        SetItems();
        SetCharacters();

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        #region CountDownTimer
        m_CountDownValue -= Time.deltaTime;
        m_CountDownValueInt = (int)m_CountDownValue;
        m_CountDownText.text = m_CountDownValueInt.ToString();
        #endregion

        if (m_GatherVotes)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                    m_PollAnswers.Add(keyCode.ToString());
            }
        }
    }

    #region SetterFunctions

    private void SetDay()
    {
        m_DayText.text = "DAY " + m_Day;
    }

    private void SetTemperature()
    {
        m_TemperatureText.text = m_Temperature + "°C";
    }

    private void SetRessources()
    {
        m_FoodText.text = m_FoodValue + "x";
        m_FirewoodText.text = m_FirewoodValue + "x";
        m_MedPackText.text = m_MedPackValue + "x";
    }

    private void SetItems()
    {

    }

    private void SetCharacters()
    {

    }

    #endregion

    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(InformationScreen());
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());
        yield return StartCoroutine(DayEnding());

        if (m_ActiveCharacters.Count == 0)
        {
            // END THE GAME
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator InformationScreen()
    {
        m_Day++;
        SetDay();

        InformationEvent tempEvent = m_InformationEvents[Random.Range(0, m_InformationEvents.Count)];
        m_CountDownValue = tempEvent.m_EventLength;

        m_InformationPanel.SetActive(true);
        m_InformationPanel.GetComponentInChildren<TextMeshProUGUI>().text = tempEvent.m_DescriptionText;

        yield return new WaitForSeconds(m_CountDownValue);
        m_InformationPanel.SetActive(false);
    }

    private IEnumerator DayStarting()
    {
        m_CountDownValue = m_DayStartingLength;

        CalculateAndSetDailyValues();

        yield return m_WaitForDayStarting;
    }

    private IEnumerator DayPlaying()
    {
        m_GatherVotes = true;

        for (int i = 0; i < m_ActiveCharacters.Count; i++)
        {
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FoodEvent"), m_ActiveCharacters[i]));
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "MedPackEvent"), m_ActiveCharacters[i]));
        }

        yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FireWoodEvent")));

        for (int j = 0; j < m_ActiveCharacters.Count; j++)
        {
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "EndOfDayEvent"), m_ActiveCharacters[j]));
        }

        m_CountDownValue = m_DayPlayingLength;
        yield return m_WaitForDayPlaying;
    }

    private IEnumerator DayEnding()
    {
        m_CountDownValue = m_DayEndingLength;

        yield return m_WaitForDayEnding;
    }

    #endregion

    #region InstantiationMethods

    public void InstantiateNewCharacter(string characterName)
    {
        Character characterSO;

        for (int i = 0; i < m_AllCharacters.Count; i++)
        {
            if (m_AllCharacters[i].m_CharacterName == characterName && m_AllCharacters[i].m_InUse == false && m_ActiveCharacters.Count < 6)
            {
                m_AllCharacters[i].m_InUse = true;
                characterSO = m_AllCharacters[i];

                CharacterManager tempCharacter = new CharacterManager(characterSO);

                tempCharacter.m_Instance = Instantiate(m_CharacterCardPrefab, m_CharacterCardPanel.transform) as GameObject;
                tempCharacter.Setup(m_ActiveCharacters.Count);
                m_ActiveCharacters.Add(tempCharacter);
            }
        }
    }

    public void InstantiateNewItem(string itemName)
    {
        Item itemSO;

        for (int i = 0; i < m_AllItems.Count; i++)
        {
            if (m_AllItems[i].m_ItemName == itemName && m_ActiveItems.Count < 6)
            {
                itemSO = m_AllItems[i];

                ItemManager tempItem = new ItemManager(itemSO);

                tempItem.m_Instance = Instantiate(m_ItemPrefab, m_ItemPanel.transform) as GameObject;
                tempItem.Setup(m_ActiveItems.Count);
                m_ActiveItems.Add(tempItem);
            }
        }
    }

    #endregion

    private void CalculateAndSetDailyValues()
    {
        float accumalatedMoraleItemFactors = 0;
        float accumalatedFullItemFactors = 0;
        float accumalatedWarmthItemFactors = 0;

        for (int j = 0; j < m_ActiveItems.Count; j++)
        {
            accumalatedMoraleItemFactors += m_ActiveItems[j].m_MoraleFactorChangeValue;
            accumalatedFullItemFactors += m_ActiveItems[j].m_FullFactorChangeValue;
            accumalatedWarmthItemFactors += m_ActiveItems[j].m_WarmthFactorChangeValue;
        }

        for (int i = 0; i < m_ActiveCharacters.Count; i++)
        {
            m_ActiveCharacters[i].SetNewCharacterValues(accumalatedMoraleItemFactors, accumalatedFullItemFactors, accumalatedWarmthItemFactors);
        }
    }

    #region PollMethods
    private IEnumerator DoPoll(Event eventV, CharacterManager characterV)
    {
        numberOfValidAnswers = 0;

        switch (eventV.name)
        {
            #region Food
            case "FoodEvent":
                m_CountDownValue = eventV.m_EventLength;
                Debug.Log("Food Event for " + characterV.m_CharacterName + ". Length: " + eventV.m_EventLength + " seconds");

                m_GatherVotes = true;
                yield return new WaitForSeconds(eventV.m_EventLength);

                CalculateAnswers(eventV);

                if (m_ListOfValidAnswersDivided[0].Count > m_ListOfValidAnswersDivided[1].Count)
                {
                    characterV.AddFull();
                    m_FoodValue--;
                    m_FoodText.text = m_FoodValue.ToString() + "x";
                }

                m_GatherVotes = false;

                m_PollAnswers.Clear();
                m_ListOfValidAnswersDivided.Clear();
                numberOfValidAnswers = 0;

                break;
            #endregion

            #region MedPack
            case "MedPackEvent":
                m_CountDownValue = eventV.m_EventLength;
                Debug.Log("MedPack Event for " + characterV.m_CharacterName + ". Length: " + eventV.m_EventLength + " seconds");

                m_GatherVotes = true;
                yield return new WaitForSeconds(eventV.m_EventLength);

                CalculateAnswers(eventV);

                if (m_ListOfValidAnswersDivided[0].Count > m_ListOfValidAnswersDivided[1].Count)
                {
                    // MED BEHAVIOUR
                }

                m_GatherVotes = false;

                m_PollAnswers.Clear();
                m_ListOfValidAnswersDivided.Clear();
                numberOfValidAnswers = 0;

                break;
            #endregion

            #region EndOfDay
            case "EndOfDayEvent":
                m_CountDownValue = eventV.m_EventLength;
                Debug.Log("EndOfDayEvent. Length: " + eventV.m_EventLength + " seconds");

                m_GatherVotes = true;
                yield return new WaitForSeconds(eventV.m_EventLength);

                CalculateAnswers(eventV);

                /*
                 * 
                 * END OF DAY BEHAVIOUR
                 * 
                 */

                m_GatherVotes = false;

                m_PollAnswers.Clear();
                m_ListOfValidAnswersDivided.Clear();
                numberOfValidAnswers = 0;

                break;
                #endregion
        }
        yield return null;
    }

    private IEnumerator DoPoll(Event eventV)
    {
        numberOfValidAnswers = 0;
        
        switch (eventV.name)
        {
            #region FireWood
            case "FireWoodEvent":
                m_CountDownValue = eventV.m_EventLength;
                Debug.Log("EndOfDayEvent. Length: " + eventV.m_EventLength + " seconds");

                m_GatherVotes = true;
                yield return new WaitForSeconds(eventV.m_EventLength);

                CalculateAnswers(eventV);

                if (m_ListOfValidAnswersDivided[0].Count > m_ListOfValidAnswersDivided[1].Count)
                {
                    m_FireWoodStrength = 3;
                }

                m_GatherVotes = false;

                m_PollAnswers.Clear();
                m_ListOfValidAnswersDivided.Clear();
                numberOfValidAnswers = 0;

                break;
                #endregion
        }
        yield return null;
    }
    #endregion

    
    void CalculateAnswers(Event eventV)
    {
        for (int i = 0; i < eventV.m_PossibleAnswers.Count; i++)
        {
            m_ListOfValidAnswersDivided.Add(new List<string>());
        }

        for (int j = 0; j < m_PollAnswers.Count; j++)
        {
            for (int k = 0; k < eventV.m_PossibleAnswers.Count; k++)
            {
                if (m_PollAnswers[j] == eventV.m_PossibleAnswers[k])
                {
                    m_ListOfValidAnswersDivided[k].Add(m_PollAnswers[j]);
                    numberOfValidAnswers++;
                }
            }
        }
    }
}