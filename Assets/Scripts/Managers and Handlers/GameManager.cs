using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Delay Lengths")]

    [SerializeField] private float m_MiniDelayLength;
    [SerializeField] private float m_ShortDelayLength;
    [SerializeField] private float m_MediumDelayLength;
    [SerializeField] private float m_LongDelayLength;

    [Header("Ressource Values")]

    [SerializeField] private int m_FoodValue;
    [SerializeField] private int m_FirewoodValue;
    [SerializeField] private int m_MedPackValue;

    [FormerlySerializedAs("DailyFireValueLoss")]
    [Header("Other")]

    [SerializeField] private float m_DailyFireValueLoss;
    
    // Manager/Handler Scripts
    [HideInInspector] public InformationManager m_InformationManager;
    [HideInInspector] public CharacterHandler m_CharacterHandler;
    [HideInInspector] public ItemHandler m_ItemHandler;
    [HideInInspector] public InterfaceHandler m_InterfaceHandler;
    [HideInInspector] public EventHandler m_EventHandler;
    [HideInInspector] public PollHandler m_PollHandler;
    [HideInInspector] public ArrowHandler m_ArrowHandler;

    // Variables
    private int m_Day;
    private int m_Temperature;
    private int m_CountDownValueInt;
    private float m_FireWoodStrength = 2f;
    private float m_CountDownValue;
    private bool m_firstRun = true;

    // For Comparability with Ressource Values
    [HideInInspector] public int m_OldFoodValue;
    [HideInInspector] public int m_OldFirewoodValue;
    [HideInInspector] public int m_OldMedPackValue;
    
    // Pre-defined WaitForSecond yields for easier Reusability
    private WaitForSeconds m_MiniWait;
    private WaitForSeconds m_ShortWait;
    private WaitForSeconds m_MediumWait;
    private WaitForSeconds m_LongWait;

    // Singleton GameManager Instance
    private static GameManager m_instance;
    public static GameManager Instance { get { return m_instance; } }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            m_instance = this;
        }

        m_MiniWait = new WaitForSeconds(m_MiniDelayLength);
        m_ShortWait = new WaitForSeconds(m_ShortDelayLength);
        m_MediumWait = new WaitForSeconds(m_MediumDelayLength);
        m_LongWait = new WaitForSeconds(m_LongDelayLength);

        m_CharacterHandler = GetComponent<CharacterHandler>();
        m_ItemHandler = GetComponent<ItemHandler>();
        m_InterfaceHandler = GetComponent<InterfaceHandler>();
        m_EventHandler = GetComponent<EventHandler>();
        m_PollHandler = GetComponent<PollHandler>();
        m_ArrowHandler = GetComponent<ArrowHandler>();

        OnRessourceValueChange += VariableChangeRessourcesHandler;
    }

    private void Start()
    {
        InstantiateGame();
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        CountDown();

        if (m_PollHandler.m_GatherVotes)
            m_PollHandler.CalculateAnswers();
    }

    private void CountDown()
    {
        m_CountDownValue -= Time.deltaTime;
        m_CountDownValueInt = (int)m_CountDownValue;
        m_InterfaceHandler.SetCountDownValue(m_CountDownValueInt);
    }

    private void InstantiateGame()
    {
        while (m_CharacterHandler.m_ActiveCharacters.Count < 2)
        {
            m_CharacterHandler.InstantiateNewCharacter(m_CharacterHandler.m_AllCharacters[Random.Range(0, m_CharacterHandler.m_AllCharacters.Count)].m_CharacterName);
        }

        SetNewTemperature();
        SetRessources();
    }

    #region GetterSetterFunctions

    public float CountDownValue
    {
        get { return m_CountDownValue; }
        set
        {
            m_CountDownValue = value;
        }
    }

    public float FirewoodStrengthValue
    {
        get { return m_FireWoodStrength; }
        set
        {
            m_FireWoodStrength = value;
        }
    }

    public int FoodValue
    {
        get { return m_FoodValue; }
        set
        {
            if (m_FoodValue == value) return;
            m_FoodValue = value;
            m_InterfaceHandler.m_FoodText.text = m_FoodValue.ToString() + "x";
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
            m_InterfaceHandler.m_MedPackText.text = m_MedPackValue.ToString() + "x";
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
            m_InterfaceHandler.m_FirewoodText.text = m_FirewoodValue.ToString() + "x";
            m_OldFirewoodValue = m_FirewoodValue;
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(m_FirewoodValue, "Firewood");
        }
    }

    #endregion

    public void SetNewTemperature()
    {
        m_Temperature = (int)(m_FireWoodStrength * 8);
        m_InterfaceHandler.m_TemperatureText.text = m_Temperature + "°C";
    }

    private void SetRessources()
    {
        m_OldFoodValue = m_FoodValue;
        m_OldFirewoodValue = m_FirewoodValue;
        m_OldMedPackValue = m_MedPackValue;

        m_InterfaceHandler.SetRessourceText();
    }

    private void SetNewCharacterValues()
    {
        float accumulatedMoraleItemFactors = 0;
        float accumulatedFullItemFactors = 0;
        float accumulatedWarmthItemFactors = 0;

        foreach (var item in m_ItemHandler.m_ActiveItems)
        {
            accumulatedMoraleItemFactors += item.m_MoraleFactorChangeValue;
            accumulatedFullItemFactors += item.m_FullFactorChangeValue;
            accumulatedWarmthItemFactors += item.m_WarmthFactorChangeValue;
        }

        foreach (var character in m_CharacterHandler.m_ActiveCharacters)
        {
            character.SetNewCharacterValues(accumulatedMoraleItemFactors, accumulatedFullItemFactors, accumulatedWarmthItemFactors);
        }
    }

    public void VariableChangeRessourcesHandler(int newVal, string valueName)
    {
        m_ArrowHandler.ArrowDisplayRessources(newVal, valueName);
    }

    public delegate void OnVariableChangeDelegate(int newVal, string valueName);
    public event OnVariableChangeDelegate OnRessourceValueChange = delegate { };
    
    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(NewDay());

        if (!m_firstRun)
        {
            yield return StartCoroutine(InformationScreen());

            if (m_CharacterHandler.m_ActiveCharacters.Count == 0)
            {
                m_InterfaceHandler.m_LosePanel.SetActive(true);
            }
        }
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());

        if(m_Day == 5)
        {
            m_InterfaceHandler.m_WinPanel.SetActive(true);
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
        m_InterfaceHandler.m_DayText.text = "DAY " + m_Day;
        m_InterfaceHandler.m_DayPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + m_Day.ToString();
        m_InterfaceHandler.m_DayPanel.SetActive(true);

        m_FireWoodStrength -= m_DailyFireValueLoss;

        m_CountDownValue = m_MiniDelayLength;
        yield return m_MiniWait;
    }

    private IEnumerator InformationScreen()
    {
        m_InformationManager.ExecuteInformationWindow();

        m_InterfaceHandler.m_InformationPanel.SetActive(true);

        m_CountDownValue = m_LongDelayLength;
        yield return m_LongWait;

        m_InterfaceHandler.m_InformationPanel.SetActive(false);
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

        /*******
            THE BEHAVIOUR OF THE DAY WITH FREE STORY TELLING PARTS AND EVENT EXECUTIONS!
            USE THE EVENT HANDLER AS YOU SEE FIT
        *******/

        m_PollHandler.m_GatherVotes = true;

        foreach (var character in m_CharacterHandler.m_ActiveCharacters)
        {
            if(FoodValue > 0)
            {
                yield return StartCoroutine(m_PollHandler.DoPoll(m_EventHandler.m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FoodEvent"), character));
                yield return StartCoroutine(AfterQuestion());
            }
                
            if(MedPackValue > 0)
            {
                yield return StartCoroutine(m_PollHandler.DoPoll(m_EventHandler.m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "MedPackEvent"), character));
                yield return StartCoroutine(AfterQuestion());
            }
        }

        foreach (var character in m_CharacterHandler.m_ActiveCharacters)
        {
            yield return StartCoroutine(m_PollHandler.DoPoll(m_EventHandler.m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "EndOfDayEvent"), character));
            yield return StartCoroutine(AfterQuestion());
        }

        if(FirewoodValue > 0)
        {
            yield return StartCoroutine(m_PollHandler.DoPoll(m_EventHandler.m_AllEvents.Find(m_AllEvents => m_AllEvents.name == "FireWoodEvent")));
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
}