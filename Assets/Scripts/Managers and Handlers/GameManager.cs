using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TwitchChatter;

public class GameManager : MonoBehaviour
{
    [Header("Sequence Lengths")]

    [SerializeField] float m_MiniDelayLength;
    [SerializeField] float m_ShortDelayLength;
    [SerializeField] float m_MediumDelayLength;
    [SerializeField] float m_LongDelayLength;

    [Header("Ressource Values")]

    [SerializeField] int m_FoodValue;
    [SerializeField] int m_FirewoodValue;
    [SerializeField] int m_MedPackValue;

    [Header("Other")]

    [SerializeField] float DailyFireValueLoss;

    TextHandler m_TextHandler;
    CharacterHandler characterHandler;
    EventHandler eventHandler;

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

    public delegate void ArrowAction();
    public event ArrowAction OnArrowCall;

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

    
    // General Variables
    private int m_Day = 0;
    private int m_Temperature;
    private bool m_firstRun = true;
    
    // For Comparibility with Ressource Values
    private int m_OldFoodValue;
    private int m_OldFirewoodValue;
    private int m_OldMedPackValue;

    // Pre-defined WaitForSecond yields for easier Reusability
    private WaitForSeconds m_MiniWait;
    private WaitForSeconds m_ShortWait;
    private WaitForSeconds m_MediumWait;
    private WaitForSeconds m_LongWait;
    
    // Singleton GameManager Instance
    private static GameManager m_instance;
    [HideInInspector]
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

        m_MiniWait = new WaitForSeconds(m_MiniDelayLength);
        m_ShortWait = new WaitForSeconds(m_ShortDelayLength);
        m_MediumWait = new WaitForSeconds(m_MediumDelayLength);
        m_LongWait = new WaitForSeconds(m_LongDelayLength);

        OnRessourceValueChange += VariableChangeRessourcesHandler;
    }

    void Start()
    {
        InstantiateGame();
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        CountDown();

        if (m_GatherVotes)
            CalculateAnswers();
    }

    void CountDown()
    {
        m_CountDownValue -= Time.deltaTime;
        m_CountDownValueInt = (int)m_CountDownValue;
        m_CountDownText.text = m_CountDownValueInt.ToString();
    }

    void InstantiateGame()
    {
        while (m_ActiveCharacters.Count < 2)
        {
            InstantiateNewCharacter(m_AllCharacters[Random.Range(0, m_AllCharacters.Count)].m_CharacterName);
        }

        SetNewTemperature();
        SetRessources();
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
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(m_FoodValue, "Food");
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
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(m_MedPackValue, "MedPack");
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
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(m_FirewoodValue, "Firewood");
        }
    }

    public void VariableChangeRessourcesHandler(int newVal, string valueName)
    {
        ArrowDisplayRessources(newVal, valueName);
        //OnArrowCall();
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
    public event OnVariableChangeDelegate OnRessourceValueChange = delegate { };

    #endregion
    
    private void SetRessources()
    {
        m_OldFoodValue = m_FoodValue;
        m_OldFirewoodValue = m_FirewoodValue;
        m_OldMedPackValue = m_MedPackValue;

        m_FoodText.text = m_FoodValue + "x";
        m_FirewoodText.text = m_FirewoodValue + "x";
        m_MedPackText.text = m_MedPackValue + "x";
    }

    private void SetNewCharacterValues()
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

    public void SetNewTemperature()
    {
        m_Temperature = (int)(m_FireWoodStrength * 8);
        m_TemperatureText.text = m_Temperature + "°C";
    }

    #endregion

    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(NewDay());

        if (!m_firstRun)
        {
            yield return StartCoroutine(InformationScreen());

            if (m_ActiveCharacters.Count == 0)
            {
                m_LosePanel.SetActive(true);
            }
        }
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());

        if(m_Day == 5)
        {
            m_WinPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator NewDay()
    {
        m_Day++;
        m_DayText.text = "DAY " + m_Day;
        m_DayPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + m_Day.ToString();
        m_DayPanel.SetActive(true);

        m_FireWoodStrength -= DailyFireValueLoss;

        m_CountDownValue = m_MiniDelayLength;
        yield return m_MiniWait;
    }

    private IEnumerator InformationScreen()
    {
        m_InformationManager.ExecuteInformationWindow();
        
        m_InformationPanel.SetActive(true);

        m_CountDownValue = m_LongDelayLength;
        yield return m_LongWait;

        m_InformationPanel.SetActive(false);
    }

    private IEnumerator DayStarting()
    {
        SetNewCharacterValues();
        SetNewTemperature();

        m_CountDownValue = m_MiniDelayLength;
        yield return m_MiniWait;
    }

    private IEnumerator DayPlaying()
    {
        m_GatherVotes = true;

        for (int i = 0; i < m_ActiveCharacters.Count; i++)
        {
            if(FoodValue > 0)
            {
                yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FoodEvent"), m_ActiveCharacters[i]));
                yield return StartCoroutine(AfterQuestion());
            }
                
            if(MedPackValue > 0)
            {
                yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "MedPackEvent"), m_ActiveCharacters[i]));
                yield return StartCoroutine(AfterQuestion());
            }
        }

        for (int j = 0; j < m_ActiveCharacters.Count; j++)
        {
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "EndOfDayEvent"), m_ActiveCharacters[j]));
            yield return StartCoroutine(AfterQuestion());
        }

        if(FirewoodValue > 0)
        {
            yield return StartCoroutine(DoPoll(m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FireWoodEvent")));
            yield return StartCoroutine(AfterQuestion());
        }

        m_CountDownValue = m_MiniDelayLength;
        yield return m_MiniWait;
    }

    private IEnumerator AfterQuestion()
    {
        m_CountDownValue = m_MiniDelayLength;
        yield return m_MiniWait;
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