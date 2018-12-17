using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TwitchChatter;

public class GameManager : MonoBehaviour
{
    public int m_Day;
    public int m_Temperature;

    public int m_FoodValue;
    private int m_OldFoodValue;
    public int m_FirewoodValue;
    private int m_OldFirewoodValue;
    public int m_MedPackValue;
    private int m_OldMedPackValue;

    public float DailyFireValueLoss;

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
    public TextMeshProUGUI m_QuestionText;

    public ParticleSystem m_FoodArrowGreen;
    public ParticleSystem m_FoodArrowRed;
    public ParticleSystem m_FirewoodArrowGreen;
    public ParticleSystem m_FirewoodArrowRed;
    public ParticleSystem m_MedPackArrowGreen;
    public ParticleSystem m_MedPackArrowRed;

    public GameObject m_AnswersPanel;
    public GameObject m_AnswerPrefab;
    public GameObject m_ResultPanel;
    public GameObject m_SliderPrefab;

    public GameObject m_CharacterCardPanel;
    public GameObject m_CharacterCardPrefab;
    public List<Character> m_AllCharacters = new List<Character>();
    public List<CharacterManager> m_ActiveCharacters = new List<CharacterManager>();

    public GameObject m_ItemPanel;
    public GameObject m_ItemPrefab;
    public List<Item> m_AllItems = new List<Item>();
    public List<ItemManager> m_ActiveItems = new List<ItemManager>();

    public GameObject m_WinPanel;
    public GameObject m_LosePanel;
    public GameObject m_DayPanel;

    public GameObject m_InformationPanel;
    public List<Event> m_AllEvents = new List<Event>();

    public List<string> m_VotersList = new List<string>();

    public List<string> m_PollAnswers = new List<string>();
    private List<List<string>> m_ListOfValidAnswersDivided = new List<List<string>>();

    public float m_CountDownValue;
    private int m_CountDownValueInt;

    public float m_FireWoodStrength = 2f;

    public bool m_GatherVotes = false;
    
    public Event m_CurrentEvent;
    public CharacterManager m_CurrentCharacter;
    public List<string> m_CurrentPossibleAnswers = new List<string>();

    public InformationManager m_InformationManager;

    private int numberOfValidAnswers;

    private bool firstRun = true;

    private WaitForSeconds m_WaitForInformationScreen;
    private WaitForSeconds m_WaitForDayStarting;
    private WaitForSeconds m_WaitForDayPlaying;
    private WaitForSeconds m_WaitForDayEnding;
    
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }

        m_WaitForInformationScreen = new WaitForSeconds(m_InformationScreenLength);
        m_WaitForDayStarting = new WaitForSeconds(m_DayStartingLength);
        m_WaitForDayPlaying = new WaitForSeconds(m_DayPlayingLength);
        m_WaitForDayEnding = new WaitForSeconds(m_DayEndingLength);

        OnVariableChangeRessourcesValues += VariableChangeRessourcesHandler;
    }

    void Start()
    {
        while (m_ActiveCharacters.Count < 2)
        {
            InstantiateNewCharacter(m_AllCharacters[Random.Range(0, m_AllCharacters.Count)].m_CharacterName);
        }

        SetDay();
        CalculateAndSetTemperature();
        SetRessources();
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
            CalculateAnswers();
        }
    }

    #region GetterSetterFunctions

    #region Ressources

    public int FoodValue
    {
        get { return m_FoodValue; }
        set
        {
            if (m_FoodValue == value) return;
            m_FoodValue = value;
            m_FoodText.text = m_FoodValue.ToString() + "x";
            m_OldFoodValue = m_FoodValue;
            if (OnVariableChangeRessourcesValues != null)
                OnVariableChangeRessourcesValues(m_FoodValue, "Food");
        }
    }

    public int MedPackValue
    {
        get { return m_MedPackValue; }
        set
        {
            if (m_MedPackValue == value) return;
            m_MedPackValue = value;
            m_MedPackText.text = m_MedPackValue.ToString() + "x";
            m_OldMedPackValue = m_MedPackValue;
            if (OnVariableChangeRessourcesValues != null)
                OnVariableChangeRessourcesValues(m_MedPackValue, "MedPack");
        }
    }

    public int FirewoodValue
    {
        get { return m_FirewoodValue; }
        set
        {
            if (m_FirewoodValue == value) return;
            m_FirewoodValue = value;
            m_FirewoodText.text = m_FirewoodValue.ToString() + "x";
            m_OldFirewoodValue = m_FirewoodValue;
            if (OnVariableChangeRessourcesValues != null)
                OnVariableChangeRessourcesValues(m_FirewoodValue, "Firewood");
        }
    }

    public void VariableChangeRessourcesHandler(int newVal, string valueName)
    {
        ArrowDisplayRessources(newVal, valueName);
    }

    public void ArrowDisplayRessources(int newVal, string valueName)
    {
        switch (valueName)
        {
            case "Food":
                if (newVal > m_OldFoodValue)
                {
                    if (m_FoodArrowGreen != null)
                        m_FoodArrowGreen.Play();
                }

                else
                {
                    if (m_FoodArrowRed != null)
                        m_FoodArrowRed.Play();
                }

                m_OldFoodValue = newVal;
                break;

            case "MedPack":
                if (newVal > m_OldMedPackValue)
                {
                    if (m_MedPackArrowGreen != null)
                        m_MedPackArrowGreen.Play();
                }

                else
                {
                    if (m_MedPackArrowRed != null)
                        m_MedPackArrowRed.Play();
                }

                m_OldMedPackValue = newVal;
                break;

            case "Firewood":
                if (newVal > m_OldFirewoodValue)
                {
                    if (m_FirewoodArrowGreen != null)
                        m_FirewoodArrowGreen.Play();
                }
                  
                else
                {
                    if (m_FirewoodArrowRed != null)
                        m_FirewoodArrowRed.Play();
                }
                    

                m_OldFirewoodValue = newVal;
                break;
        }
    }

    public delegate void OnVariableChangeDelegate(int newVal, string valueName);
    public event OnVariableChangeDelegate OnVariableChangeRessourcesValues;

    #endregion

    private void SetDay()
    {
        m_DayText.text = "DAY " + m_Day;
    }

    private void SetRessources()
    {
        m_OldFoodValue = m_FoodValue;
        m_OldFirewoodValue = m_FirewoodValue;
        m_OldMedPackValue = m_MedPackValue;

        m_FoodText.text = m_FoodValue + "x";
        m_FirewoodText.text = m_FirewoodValue + "x";
        m_MedPackText.text = m_MedPackValue + "x";
    }

    private void SetCharacters()
    {

    }

    private void CalculateAndSetCharacterValues()
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

    public void CalculateAndSetTemperature()
    {
        m_Temperature = (int)(m_FireWoodStrength * 8);
        m_TemperatureText.text = m_Temperature + "°C";
    }

    #endregion

    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(DayScreen());

        if (!firstRun)
        {
            yield return StartCoroutine(InformationScreen());

            if (m_ActiveCharacters.Count == 0)
            {
                m_LosePanel.SetActive(true);
            }
        }
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());
        yield return StartCoroutine(DayEnding());

        if(m_Day == 10)
        {
            m_WinPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator DayScreen()
    {
        m_Day++;
        SetDay();

        m_DayPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + m_Day.ToString();
        m_DayPanel.SetActive(true);

        m_CountDownValue = 3;
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator InformationScreen()
    {
        m_InformationManager.ExecuteInformationWindow();

        m_CountDownValue = 23f;

        m_InformationPanel.SetActive(true);
        
        yield return new WaitForSeconds(m_CountDownValue);
        m_InformationPanel.SetActive(false);
    }

    private IEnumerator DayStarting()
    {
        m_CountDownValue = m_DayStartingLength;

        CalculateAndSetCharacterValues();
        CalculateAndSetTemperature();

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

        for (int j = 0; j < m_ActiveCharacters.Count; j++)
        {
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "EndOfDayEvent"), m_ActiveCharacters[j]));
        }

        yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FireWoodEvent")));

        m_CountDownValue = m_DayPlayingLength;
        yield return m_WaitForDayPlaying;
    }

    private IEnumerator DayEnding()
    {
        m_CountDownValue = m_DayEndingLength;

        m_FireWoodStrength -= DailyFireValueLoss;

        yield return m_WaitForDayEnding;
        firstRun = false;
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
                //tempCharacter.OnVariableChange += VariableCahng
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

    #region PollMethods
    private IEnumerator DoPoll(Event eventV, CharacterManager characterV)
    {
        m_CurrentEvent = eventV;
        m_CurrentCharacter = characterV;
        m_CurrentPossibleAnswers = eventV.m_PossibleAnswers;

        eventV.Instantiate();
        m_GatherVotes = true;

        yield return new WaitForSeconds(eventV.m_EventLength);

        CalculateAnswers();
        eventV.Execute(m_ListOfValidAnswersDivided, characterV);
        m_GatherVotes = false;

        ResetForNewEvent();
    }

    private IEnumerator DoPoll(Event eventV)
    {
        m_CurrentEvent = eventV;
        m_CurrentPossibleAnswers = eventV.m_PossibleAnswers;

        eventV.Instantiate();
        m_GatherVotes = true;

        yield return new WaitForSeconds(eventV.m_EventLength);

        CalculateAnswers();
        eventV.Execute(m_ListOfValidAnswersDivided);
        m_GatherVotes = false;

        ResetForNewEvent();
    }
    #endregion

    void CalculateAnswers()
    {
        m_ListOfValidAnswersDivided.Clear();

        for (int i = 0; i < m_CurrentEvent.m_PossibleAnswers.Count; i++)
        {
            m_ListOfValidAnswersDivided.Add(new List<string>());
        }

        for (int j = 0; j < m_PollAnswers.Count; j++)
        {
            for (int k = 0; k < m_CurrentEvent.m_PossibleAnswers.Count; k++)
            {
                if (m_PollAnswers[j].ToLower() == m_CurrentEvent.m_PossibleAnswers[k].ToLower())
                {
                    m_ListOfValidAnswersDivided[k].Add(m_PollAnswers[j].ToLower());
                    numberOfValidAnswers++;
                }
            }
        }

        if (m_PollAnswers.Count != 0)
        {
            for (int l = 0; l < m_ListOfValidAnswersDivided.Count; l++)
            {
                float newValue = (float)m_ListOfValidAnswersDivided[l].Count / (float)m_PollAnswers.Count;
                m_ResultPanel.transform.GetChild(l).GetComponentInChildren<Slider>().value = newValue;
            }
        }
    }
    
    void ResetForNewEvent()
    {
        foreach (Transform child in m_AnswersPanel.transform)
        {
            Destroy(child.gameObject);
        }

        m_QuestionText.text = null;

        foreach (Transform child in m_ResultPanel.transform)
        {
            Destroy(child.gameObject);
        }

        m_CurrentCharacter = null;
        m_CurrentEvent = null;

        m_VotersList.Clear();
        m_PollAnswers.Clear();
        m_ListOfValidAnswersDivided.Clear();
        m_InformationManager.Reset();
    }
}