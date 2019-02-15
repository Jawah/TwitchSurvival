using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CharacterManager
{
    public string characterName;
    public int characterNumber;
    public Sprite characterSprite;

    public GameObject Instance;

    public float moraleValue;
    private float _oldMoraleValue;
    public float fullValue;
    private float _oldFullValue;
    public float warmthValue;
    private float _oldWarmthValue;

    public int skillPlunder;
    public int skillTimber;
    
    public float moraleLossFactor;
    public float fullLossFactor;
    public float warmthLossFactor;

    public float healthStatusMoraleLossFactor = 1;
    public float healthStatusFullLossFactor = 1;
    public float healthStatusWarmthLossFactor = 1;

    public float fullGainValue;
    public float warmthGainValue;

    public bool isPlundering;

    public bool isSick;

    public enum PlayerState { Default, Plunder, ChopWood };
    public PlayerState playerState = PlayerState.Default;

    public enum HealthState { Default, Sick, Depressed, Fracture };
    public HealthState healthState = HealthState.Default;

    private Character _character;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _healthStatusText;
    private TextMeshProUGUI _moraleValueText;
    private TextMeshProUGUI _fullValueText;
    private TextMeshProUGUI _warmthValueText;

    private TextMeshProUGUI _quoteText;

    private ParticleSystem _moraleArrowGreen;
    private ParticleSystem _moraleArrowRed;
    private ParticleSystem _fullArrowGreen;
    private ParticleSystem _fullArrowRed;
    private ParticleSystem _warmthArrowGreen;
    private ParticleSystem _warmthArrowRed;

    private Image _characterImage;

    public CharacterManager(Character characterSO)
    {
        _character = characterSO;
    }

    public void Setup(int characterNum)
    {
        _oldMoraleValue = moraleValue;
        _oldFullValue = fullValue;
        _oldWarmthValue = warmthValue;
        
        OnVariableChangeCharacterValues += VariableChangeHandler;

        characterNumber = characterNum;
        characterName = _character.characterName;
        moraleLossFactor = _character.dailyMoraleLossFactor;
        fullLossFactor = _character.dailyFullLossFactor;
        characterSprite = _character.characterSprite;
        warmthLossFactor = _character.dailyWarmthLossFactor;
        moraleValue = Random.Range(3.3f, 7f) + 2f;
        fullValue = Random.Range(3.3f, 7f) + 2f;
        warmthValue = Random.Range(3.3f, 7f) + 2f;
        _oldMoraleValue = moraleValue;
        _oldFullValue = fullValue;
        _oldWarmthValue = warmthValue;

        skillTimber = _character.skillTimber;
        skillPlunder = _character.skillPlunder;
        
        fullGainValue = _character.fullGainValue;
        warmthGainValue = _character.warmthGainValue;

        var textChildren = Instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var child in textChildren)
        {
            if (child.gameObject.CompareTag(Tags.NAME_TEXT_TAG))
                _nameText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.MORALE_VALUE_TEXT_TAG))
                _moraleValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.FULL_VALUE_TEXT_TAG))
                _fullValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.WARMTH_VALUE_TEXT_TAG))
                _warmthValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.HEALTH_STATUS_TEXT_TAG))
                _healthStatusText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.QUOTE_TEXT_TAG))
                _quoteText = child.GetComponent<TextMeshProUGUI>();
        }

        var imageChildren = Instance.GetComponentsInChildren<Image>();
        foreach (var child in imageChildren)
        {
            if (child.gameObject.CompareTag(Tags.CHARACTER_IMAGE_TAG))
                _characterImage = child.GetComponent<Image>();
        }

        var particleChildren = Instance.GetComponentsInChildren<ParticleSystem>();
        foreach (var child in particleChildren)
        {
            if (child.gameObject.CompareTag(Tags.ARROW_RED_TAG))
            {
                switch (child.transform.parent.name)
                {
                    case "MoralePanel":
                        _moraleArrowRed = child.GetComponent<ParticleSystem>();
                        break;

                    case "HungerPanel":
                        _fullArrowRed = child.GetComponent<ParticleSystem>();
                        break;

                    case "WarmthPanel":
                        _warmthArrowRed = child.GetComponent<ParticleSystem>();
                        break;
                }
            }
            else if (child.gameObject.CompareTag(Tags.ARROW_GREEN_TAG))
            {
                switch (child.transform.parent.name)
                {
                    case "MoralePanel":
                        _moraleArrowGreen = child.GetComponent<ParticleSystem>();
                        break;

                    case "HungerPanel":
                        _fullArrowGreen = child.GetComponent<ParticleSystem>();
                        break;

                    case "WarmthPanel":
                        _warmthArrowGreen = child.GetComponent<ParticleSystem>();
                        break;
                }
            }
        }

        _nameText.text = characterName;

        _moraleValueText.text = moraleValue.ToString("F1");
        _fullValueText.text = fullValue.ToString("F1");
        _warmthValueText.text = warmthValue.ToString("F1");
        _characterImage.sprite = characterSprite;

        TextColorChanger(moraleValue, "Morale");
        TextColorChanger(fullValue, "Full");
        TextColorChanger(warmthValue, "Warmth");
        SetQuote();
    }

    public void VariableChangeHandler(float newVal, string valueName)
    {
        TextColorChanger(newVal, valueName);
        ArrowDisplay(newVal, valueName);
        SetValueText();
        SetQuote();
    }

    public void SetNewCharacterValues(float accumulatedMoraleItemFactors, float accumulatedFullItemFactors, float accumulatedWarmthItemFactors)
    {
        MoraleValue += ((-moraleLossFactor + accumulatedMoraleItemFactors) * healthStatusMoraleLossFactor);
        FullValue += ((-fullLossFactor + accumulatedFullItemFactors) * healthStatusFullLossFactor);
        WarmthValue += ((-warmthLossFactor + accumulatedWarmthItemFactors) * healthStatusWarmthLossFactor) + GameManager.Instance.FirewoodStrengthValue/3;

        _moraleValueText.text = moraleValue.ToString("F1");
        _fullValueText.text = fullValue.ToString("F1");
        _warmthValueText.text = warmthValue.ToString("F1");
        SetQuote();
    }

    public void SetValueText()
    {
        _moraleValueText.text = moraleValue.ToString("F1");
        _fullValueText.text = fullValue.ToString("F1");
        _warmthValueText.text = warmthValue.ToString("F1");
    }

    public void AddFull()
    {
        FullValue += fullGainValue;
        _fullValueText.text = fullValue.ToString("F1");
    }

    public void AddWarmth()
    {
        WarmthValue += warmthGainValue;
        _warmthValueText.text = warmthValue.ToString("F1");
    }

    public float MoraleValue
    {
        get { return moraleValue; }
        set
        {
            moraleValue = value;
            if (MoraleValue < 0) MoraleValue = 0;
            if (MoraleValue > 10) MoraleValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(moraleValue, "Morale");
        }
    }

    public float FullValue
    {
        get { return fullValue; }
        set
        {
            fullValue = value;
            if (FullValue < 0) FullValue = 0;
            if (FullValue > 10) FullValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(fullValue, "Full");
        }
    }

    public float WarmthValue
    {
        get { return warmthValue; }
        set
        {
            warmthValue = value;
            if (WarmthValue < 0) WarmthValue = 0;
            if (WarmthValue > 10) WarmthValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(warmthValue, "Warmth");
        }
    }

    public void StatusChanger()
    {
        switch (healthState)
        {
            case HealthState.Default:
                healthStatusMoraleLossFactor = 1;
                healthStatusFullLossFactor = 1;
                healthStatusWarmthLossFactor = 1;
                _healthStatusText.text = "FEELS GOOD";
                _healthStatusText.color = Color.green;
                break;
            case HealthState.Depressed:
                healthStatusMoraleLossFactor = 2;
                _healthStatusText.text = "IS DEPRESSED";
                _healthStatusText.color = Color.red;
                break;
            case HealthState.Fracture:
                healthStatusFullLossFactor = 2;
                _healthStatusText.text = "BROKE A LEG";
                _healthStatusText.color = Color.red;
                break;
            case HealthState.Sick:
                healthStatusWarmthLossFactor = 2;
                healthStatusFullLossFactor = 2;
                healthStatusMoraleLossFactor = 2;
                _healthStatusText.text = "IS SICK";
                _healthStatusText.color = Color.red;
                break;
        }
    }

    public void ArrowDisplay(float newVal, string valueName)
    {
        switch (valueName)
        {
            case "Morale":
                if (newVal > _oldMoraleValue)
                {
                    if(_moraleArrowGreen != null)
                        _moraleArrowGreen.Play();
                }
                else
                {
                    if(_moraleArrowRed != null)
                        _moraleArrowRed.Play();
                }

                _oldMoraleValue = newVal;
                break;

            case "Full":
                if (newVal > _oldFullValue)
                {
                    if(_fullArrowGreen != null)
                        _fullArrowGreen.Play();
                }
                else
                {
                    if(_fullArrowRed != null)
                        _fullArrowRed.Play();
                }

                _oldFullValue = newVal;
                break;

            case "Warmth":
                if (newVal > _oldWarmthValue)
                {
                    if(_warmthArrowGreen != null)
                        _warmthArrowGreen.Play();
                }
                else
                {
                    if(_warmthArrowRed != null)
                        _warmthArrowRed.Play();
                }

                _oldWarmthValue = newVal;
                break;
        }
    }

    public void TextColorChanger(float newVal, string valueName)
    {
        switch (valueName)
        {
            case "Morale":
                if (newVal < 3)
                    _moraleValueText.color = Color.red;
                else if (newVal < 7)
                    _moraleValueText.color = Color.yellow;
                else if (newVal >= 7)
                    _moraleValueText.color = Color.green;
                break;

            case "Full":
                if (newVal < 3)
                    _fullValueText.color = Color.red;
                else if (newVal < 7)
                    _fullValueText.color = Color.yellow;
                else if (newVal >= 7)
                    _fullValueText.color = Color.green;
                break;

            case "Warmth":
                if (newVal < 3)
                    _warmthValueText.color = Color.red;
                else if (newVal < 7)
                    _warmthValueText.color = Color.yellow;
                else if (newVal >= 7)
                    _warmthValueText.color = Color.green;
                break;
        }
    }

    public delegate void OnVariableChangeDelegate(float newVal, string valueName);
    public event OnVariableChangeDelegate OnVariableChangeCharacterValues;

    public void SetQuote()
    {
        int rnd = Random.Range(0, 2);

        if (rnd == 0)
        {
            if (WarmthValue < 3)
            {
                _quoteText.text = Quotes.QUOTES_WARMTH_NEGATIVE[Random.Range(0, Quotes.QUOTES_WARMTH_NEGATIVE.Length)];
                _quoteText.color = Color.red;
            }
            else if (WarmthValue < 7)
            {
                _quoteText.text = Quotes.QUOTES_WARMTH_NEUTRAL[Random.Range(0, Quotes.QUOTES_WARMTH_NEUTRAL.Length)];
                _quoteText.color = Color.yellow;
            }

            if (FullValue < 3)
            {
                _quoteText.text = Quotes.QUOTES_HUNGER_NEGATIVE[Random.Range(0, Quotes.QUOTES_HUNGER_NEGATIVE.Length)];
                _quoteText.color = Color.red;
            }
            else if (FullValue < 7)
            {
                _quoteText.text = Quotes.QUOTES_HUNGER_NEUTRAL[Random.Range(0, Quotes.QUOTES_HUNGER_NEUTRAL.Length)];
                _quoteText.color = Color.yellow;
            }
        }
        
        if (rnd == 1)
        {
            if (FullValue < 3)
            {
                _quoteText.text = Quotes.QUOTES_HUNGER_NEGATIVE[Random.Range(0, Quotes.QUOTES_HUNGER_NEGATIVE.Length)];
                _quoteText.color = Color.red;
            }
            else if (FullValue < 7)
            {
                _quoteText.text = Quotes.QUOTES_HUNGER_NEUTRAL[Random.Range(0, Quotes.QUOTES_HUNGER_NEUTRAL.Length)];
                _quoteText.color = Color.yellow;
            }
            
            if (WarmthValue < 3)
            {
                _quoteText.text = Quotes.QUOTES_WARMTH_NEGATIVE[Random.Range(0, Quotes.QUOTES_WARMTH_NEGATIVE.Length)];
                _quoteText.color = Color.red;
            }
            else if (WarmthValue < 7)
            {
                _quoteText.text = Quotes.QUOTES_WARMTH_NEUTRAL[Random.Range(0, Quotes.QUOTES_WARMTH_NEUTRAL.Length)];
                _quoteText.color = Color.yellow;
            }
        }

        if (MoraleValue >= 7 && FullValue >= 7)
        {
            _quoteText.text = Quotes.QUOTES_RANDOM[Random.Range(0, Quotes.QUOTES_RANDOM.Length)];
            _quoteText.color = Color.white;
        }
    }
}
