using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Delay Lengths")]

    [SerializeField] private float _miniDelayLength;
    [SerializeField] private float _shortDelayLength;
    [SerializeField] private float _mediumDelayLength;
    [SerializeField] private float _longDelayLength;

    [Header("Ressource Values")]

    [SerializeField] private int _foodValue;
    [SerializeField] private int _firewoodValue;
    [SerializeField] private int _medPackValue;
    [SerializeField] private float _groupMoraleValue;

    [Header("Other")]

    [SerializeField] private float _dailyFireValueLoss;
    
    // Manager/Handler Scripts
    [HideInInspector] public InformationManager informationManager;
    [HideInInspector] public ScenarioManager scenarioManager;
    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public CharacterHandler characterHandler;
    [HideInInspector] public ItemHandler itemHandler;
    [HideInInspector] public InterfaceHandler interfaceHandler;
    [HideInInspector] public EventHandler eventHandler;
    [HideInInspector] public PollHandler pollHandler;
    [HideInInspector] public ArrowHandler arrowHandler;
    
    // Variables
    private int _day;
    public int _temperature;
    private int _countDownValueInt;
    private float _fireWoodStrength = 3f;
    private float _countDownValue;
    private bool _firstRun = true;

    public int answerCount = 0;

    // For Comparability with Ressource Values
    [HideInInspector] public int oldFoodValue;
    [HideInInspector] public int oldFirewoodValue;
    [HideInInspector] public int oldMedPackValue;
    
    // Pre-defined WaitForSecond yields for easier Reusability
    private WaitForSeconds _miniWait;
    private WaitForSeconds _shortWait;
    private WaitForSeconds _mediumWait;
    private WaitForSeconds _longWait;

    // Singleton GameManager Instance
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _miniWait = new WaitForSeconds(_miniDelayLength);
        _shortWait = new WaitForSeconds(_shortDelayLength);
        _mediumWait = new WaitForSeconds(_mediumDelayLength);
        _longWait = new WaitForSeconds(_longDelayLength);

        informationManager = GameObject.Find("InformationManager").GetComponent<InformationManager>();
        scenarioManager = GameObject.Find("ScenarioManager").GetComponent<ScenarioManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        characterHandler = GetComponent<CharacterHandler>();
        itemHandler = GetComponent<ItemHandler>();
        interfaceHandler = GetComponent<InterfaceHandler>();
        eventHandler = GetComponent<EventHandler>();
        pollHandler = GetComponent<PollHandler>();
        arrowHandler = GetComponent<ArrowHandler>();

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

        if (pollHandler.gatherVotes)
            pollHandler.CalculateAnswers();
    }

    private void CountDown()
    {
        _countDownValue -= Time.deltaTime;
        _countDownValueInt = (int)_countDownValue;
        interfaceHandler.SetCountDownValue(_countDownValueInt);
    }

    private void InstantiateGame()
    {
        while (characterHandler.activeCharacters.Count < 2)
        {
            characterHandler.InstantiateNewCharacter(characterHandler.allCharacters[Random.Range(0, characterHandler.allCharacters.Count)].characterName);
        }

        SetNewTemperature();
        SetRessources();
    }

    #region GetterSetterFunctions

    public float CountDownValue
    {
        get { return _countDownValue; }
        set
        {
            _countDownValue = value;
        }
    }

    public float FirewoodStrengthValue
    {
        get { return _fireWoodStrength; }
        set
        {
            _fireWoodStrength = value;
        }
    }

    public int FoodValue
    {
        get { return _foodValue; }
        set
        {
            if (_foodValue == value) return;
            _foodValue = value;
            interfaceHandler.foodText.text = _foodValue.ToString() + "x";
            oldFoodValue = _foodValue;
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(_foodValue, "Food");
        }
    }

    public int MedPackValue
    {
        get { return _medPackValue; }
        set
        {
            if (_medPackValue == value) return;
            _medPackValue = value;
            interfaceHandler.medPackText.text = _medPackValue.ToString() + "x";
            oldMedPackValue = _medPackValue;
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(_medPackValue, "MedPack");
        }
    }

    public int FirewoodValue
    {
        get { return _firewoodValue; }
        set
        {
            if (_firewoodValue == value) return;
            _firewoodValue = value;
            interfaceHandler.firewoodText.text = _firewoodValue.ToString() + "x";
            oldFirewoodValue = _firewoodValue;
            if (OnRessourceValueChange != null)
                OnRessourceValueChange(_firewoodValue, "Firewood");
        }
    }

    #endregion

    public void SetNewTemperature()
    {
        _temperature = (int)(_fireWoodStrength * 8);
        interfaceHandler.temperatureText.text = _temperature + "°C";
    }

    private void SetRessources()
    {
        oldFoodValue = _foodValue;
        oldFirewoodValue = _firewoodValue;
        oldMedPackValue = _medPackValue;

        interfaceHandler.SetRessourceText();
    }

    private void SetNewCharacterValues()
    {
        float accumulatedMoraleItemFactors = 0;
        float accumulatedFullItemFactors = 0;
        float accumulatedWarmthItemFactors = 0;

        foreach (var item in itemHandler.activeItems)
        {
            accumulatedMoraleItemFactors += item.moraleFactorChangeValue;
            accumulatedFullItemFactors += item.fullFactorChangeValue;
            accumulatedWarmthItemFactors += item.warmthFactorChangeValue;
        }

        foreach (var character in characterHandler.activeCharacters)
        {
            character.SetNewCharacterValues(accumulatedMoraleItemFactors, accumulatedFullItemFactors, accumulatedWarmthItemFactors);
        }
    }

    private void SetNewGroupMorale()
    {
        float newGroupMoraleValue = 0;
        
        foreach (var character in characterHandler.activeCharacters)
        {
            newGroupMoraleValue += character.moraleValue;
        }

        _groupMoraleValue = newGroupMoraleValue / characterHandler.activeCharacters.Count;
        
        Debug.Log("Gruppenmoral: " + _groupMoraleValue.ToString("F1"));
    }

    public void VariableChangeRessourcesHandler(int newVal, string valueName)
    {
        arrowHandler.ArrowDisplayRessources(newVal, valueName);
    }

    public delegate void OnVariableChangeDelegate(int newVal, string valueName);
    public event OnVariableChangeDelegate OnRessourceValueChange = delegate { };
    
    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(NewDay());

        if (!_firstRun)
        {
            yield return StartCoroutine(InformationScreen());
        }
        
        yield return StartCoroutine(DayStarting());
        
        if (characterHandler.activeCharacters.Count == 0)
        {
            interfaceHandler.losePanel.SetActive(true);
            
            yield return StartCoroutine(EndGame());
        }

        
        if(_day == 6 && characterHandler.activeCharacters.Count > 0)
        {
            interfaceHandler.winPanel.SetActive(true);
            
            yield return StartCoroutine(EndGame());
        }
        
        yield return StartCoroutine(DayPlaying());

        if (_firstRun)
            _firstRun = false;
        
            StartCoroutine(GameLoop());
    }

    private IEnumerator NewDay()
    {
        _day++;
        interfaceHandler.dayText.text = _day.ToString();
        interfaceHandler.dayPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Tag " + _day.ToString();
        interfaceHandler.dayPanel.SetActive(true);

        _fireWoodStrength -= _dailyFireValueLoss;

        _countDownValue = _miniDelayLength;
        yield return _miniWait;
    }

    private IEnumerator InformationScreen()
    {
        informationManager.ExecuteInformationWindow();

        interfaceHandler.informationPanel.SetActive(true);

        _countDownValue = _longDelayLength;
        yield return _longWait;

        interfaceHandler.informationPanel.SetActive(false);
    }

    private IEnumerator DayStarting()
    {
        SetNewCharacterValues();
        SetNewGroupMorale();
        SetNewTemperature();
        
        _countDownValue = _miniDelayLength;
        yield return _miniWait;
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(600f);
        Time.timeScale = 0f;
        
    }
    
    private IEnumerator DayPlaying()
    {   
        
        
        //interfaceHandler.storyPanel.SetActive(false);
        
        pollHandler.gatherVotes = true;
        
        

        foreach (var character in characterHandler.activeCharacters)
        {  
            if(MedPackValue > 0)
            {
                if (character.healthState != CharacterManager.HealthState.Default)
                {
                    yield return StartCoroutine(pollHandler.DoPoll(eventHandler.allEvents.Find(m_AllEvents => m_AllEvents.name == "MedPackEvent"), character));
                    yield return StartCoroutine(AfterQuestion());
                }
            }
        }
        
        if(FoodValue > 0)
        {
            yield return StartCoroutine(pollHandler.DoPoll(eventHandler.allEvents.Find(m_AllEvents => m_AllEvents.name == "FoodEvent")));
            yield return StartCoroutine(AfterQuestion());
        }

        yield return StartCoroutine(scenarioManager.StartScenarioRoutine());
        
        scenarioManager.scenarioTextTyper.Type(
            "Den Rest des Tages bleibt es ruhig..."
        );
        
        yield return _shortWait;

        scenarioManager.scenarioTextTyper.newText = "";
        
        yield return StartCoroutine(interfaceHandler.DisableBigPanel());
        
        foreach (var character in characterHandler.activeCharacters)
        {
            yield return StartCoroutine(pollHandler.DoPoll(eventHandler.allEvents.Find(m_AllEvents => m_AllEvents.name == "EndOfDayEvent"), character));
            yield return StartCoroutine(AfterQuestion());
        }

        if(FirewoodValue > 0)
        {
            yield return StartCoroutine(pollHandler.DoPoll(eventHandler.allEvents.Find(m_AllEvents => m_AllEvents.name == "FireWoodEvent")));
            yield return StartCoroutine(AfterQuestion());
        }

        _countDownValue = _miniDelayLength;
        yield return _miniWait;
    }

    public IEnumerator AfterQuestion()
    {
        interfaceHandler.chosenAnswerPanel.SetActive(true);
        _countDownValue = 6f;
        interfaceHandler.PlayAnswerReveal();
        yield return _miniWait;
        interfaceHandler.chosenAnswerPanel.SetActive(false);
    }

    #endregion

    public IEnumerator CoroutineCaller(IEnumerator routine)
    {
        yield return StartCoroutine(routine);
    }
}