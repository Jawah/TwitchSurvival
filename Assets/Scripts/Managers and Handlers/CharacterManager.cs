using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CharacterManager
{
    public string m_CharacterName;
    public int m_CharacterNumber;
    public Sprite m_CharacterSprite;

    public GameObject m_Instance;

    public float m_MoraleValue;
    private float m_oldMoraleValue;
    public float m_FullValue;
    private float m_oldFullValue;
    public float m_WarmthValue;
    private float m_oldWarmthValue;

    public float m_MoraleLossFactor;
    public float m_FullLossFactor;
    public float m_WarmthLossFactor;

    public float m_HealthStatusMoraleLossFactor = 1;
    public float m_HealthStatusFullLossFactor = 1;
    public float m_HealthStatusWarmthLossFactor = 1;

    public float m_FullGainValue;
    public float m_WarmthGainValue;

    public bool m_IsPlundering;

    public bool m_IsSick;

    public enum PlayerState { Default, Plunder, ChopWood };
    public PlayerState playerState = PlayerState.Default;

    public enum HealthState { Default, Sick, Depressed, Fracture };
    public HealthState healthState = HealthState.Default;

    private Character m_Character;

    private TextMeshProUGUI m_NameText;
    private TextMeshProUGUI m_HealthStatusText;
    private TextMeshProUGUI m_MoraleValueText;
    private TextMeshProUGUI m_FullValueText;
    private TextMeshProUGUI m_WarmthValueText;

    private ParticleSystem m_MoraleArrowGreen;
    private ParticleSystem m_MoraleArrowRed;
    private ParticleSystem m_FullArrowGreen;
    private ParticleSystem m_FullArrowRed;
    private ParticleSystem m_WarmthArrowGreen;
    private ParticleSystem m_WarmthArrowRed;

    private Image m_CharacterImage;

    public CharacterManager(Character characterSO)
    {
        m_Character = characterSO;
    }

    public void Setup(int characterNumber)
    {
        m_oldMoraleValue = m_MoraleValue;
        m_oldFullValue = m_FullValue;
        m_oldWarmthValue = m_WarmthValue;
        
        OnVariableChangeCharacterValues += VariableChangeHandler;

        m_CharacterNumber = characterNumber;
        m_CharacterName = m_Character.m_CharacterName;
        m_MoraleLossFactor = m_Character.m_DailyMoraleLossFactor;
        m_FullLossFactor = m_Character.m_DailyFullLossFactor;
        m_CharacterSprite = m_Character.m_CharacterSprite;
        m_WarmthLossFactor = m_Character.m_DailyWarmthLossFactor;
        m_MoraleValue = Random.Range(3.3f, 7f) + 2f;
        m_FullValue = Random.Range(3.3f, 7f) + 2f;
        m_WarmthValue = Random.Range(3.3f, 7f) + 2f;
        m_oldMoraleValue = m_MoraleValue;
        m_oldFullValue = m_FullValue;
        m_oldWarmthValue = m_WarmthValue;

        m_FullGainValue = m_Character.m_FullGainValue;
        m_WarmthGainValue = m_Character.m_WarmthGainValue;

        var textChildren = m_Instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var child in textChildren)
        {
            if (child.gameObject.CompareTag(Tags.NAME_TEXT_TAG))
                m_NameText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.MORALE_VALUE_TEXT_TAG))
                m_MoraleValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.FULL_VALUE_TEXT_TAG))
                m_FullValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.WARMTH_VALUE_TEXT_TAG))
                m_WarmthValueText = child.GetComponent<TextMeshProUGUI>();
            else if (child.gameObject.CompareTag(Tags.HEALTH_STATUS_TEXT_TAG))
                m_HealthStatusText = child.GetComponent<TextMeshProUGUI>();
        }

        var imageChildren = m_Instance.GetComponentsInChildren<Image>();
        foreach (var child in imageChildren)
        {
            if (child.gameObject.CompareTag(Tags.CHARACTER_IMAGE_TAG))
                m_CharacterImage = child.GetComponent<Image>();
        }

        var particleChildren = m_Instance.GetComponentsInChildren<ParticleSystem>();
        foreach (var child in particleChildren)
        {
            if (child.gameObject.CompareTag(Tags.ARROW_RED_TAG))
            {
                switch (child.transform.parent.name)
                {
                    case "MoralePanel":
                        m_MoraleArrowRed = child.GetComponent<ParticleSystem>();
                        break;

                    case "HungerPanel":
                        m_FullArrowRed = child.GetComponent<ParticleSystem>();
                        break;

                    case "WarmthPanel":
                        m_WarmthArrowRed = child.GetComponent<ParticleSystem>();
                        break;
                }
            }
            else if (child.gameObject.CompareTag(Tags.ARROW_GREEN_TAG))
            {
                switch (child.transform.parent.name)
                {
                    case "MoralePanel":
                        m_MoraleArrowGreen = child.GetComponent<ParticleSystem>();
                        break;

                    case "HungerPanel":
                        m_FullArrowGreen = child.GetComponent<ParticleSystem>();
                        break;

                    case "WarmthPanel":
                        m_WarmthArrowGreen = child.GetComponent<ParticleSystem>();
                        break;
                }
            }
        }

        m_NameText.text = m_CharacterName;

        m_MoraleValueText.text = m_MoraleValue.ToString("F1");
        m_FullValueText.text = m_FullValue.ToString("F1");
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
        m_CharacterImage.sprite = m_CharacterSprite;

        TextColorChanger(m_MoraleValue, "Morale");
        TextColorChanger(m_FullValue, "Full");
        TextColorChanger(m_WarmthValue, "Warmth");
    }

    public void VariableChangeHandler(float newVal, string valueName)
    {
        TextColorChanger(newVal, valueName);
        ArrowDisplay(newVal, valueName);
    }

    public void SetNewCharacterValues(float accumulatedMoraleItemFactors, float accumulatedFullItemFactors, float accumulatedWarmthItemFactors)
    {
        MoraleValue += ((-m_MoraleLossFactor + accumulatedMoraleItemFactors) * m_HealthStatusMoraleLossFactor);
        FullValue += ((-m_FullLossFactor + accumulatedFullItemFactors) * m_HealthStatusFullLossFactor);
        WarmthValue += ((-m_WarmthLossFactor + accumulatedWarmthItemFactors) * m_HealthStatusWarmthLossFactor) + GameManager.Instance.FirewoodStrengthValue/3;

        m_MoraleValueText.text = m_MoraleValue.ToString("F1");
        m_FullValueText.text = m_FullValue.ToString("F1");
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
    }

    public void AddFull()
    {
        FullValue += m_FullGainValue;
        m_FullValueText.text = m_FullValue.ToString("F1");
    }

    public void AddWarmth()
    {
        WarmthValue += m_WarmthGainValue;
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
    }

    public float MoraleValue
    {
        get { return m_MoraleValue; }
        set
        {
            m_MoraleValue = value;
            if (MoraleValue < 0) MoraleValue = 0;
            if (MoraleValue > 10) MoraleValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(m_MoraleValue, "Morale");
        }
    }

    public float FullValue
    {
        get { return m_FullValue; }
        set
        {
            m_FullValue = value;
            if (FullValue < 0) FullValue = 0;
            if (FullValue > 10) FullValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(m_FullValue, "Full");
        }
    }

    public float WarmthValue
    {
        get { return m_WarmthValue; }
        set
        {
            m_WarmthValue = value;
            if (WarmthValue < 0) WarmthValue = 0;
            if (WarmthValue > 10) WarmthValue = 10;
            if (OnVariableChangeCharacterValues != null)
                OnVariableChangeCharacterValues(m_WarmthValue, "Warmth");
        }
    }

    public void StatusChanger()
    {
        switch (healthState)
        {
            case HealthState.Default:
                m_HealthStatusMoraleLossFactor = 1;
                m_HealthStatusFullLossFactor = 1;
                m_HealthStatusWarmthLossFactor = 1;
                m_HealthStatusText.text = "FEELS GOOD";
                m_HealthStatusText.color = Color.green;
                break;
            case HealthState.Depressed:
                m_HealthStatusMoraleLossFactor = 2;
                m_HealthStatusText.text = "IS DEPRESSED";
                m_HealthStatusText.color = Color.red;
                break;
            case HealthState.Fracture:
                m_HealthStatusFullLossFactor = 2;
                m_HealthStatusText.text = "BROKE A LEG";
                m_HealthStatusText.color = Color.red;
                break;
            case HealthState.Sick:
                m_HealthStatusWarmthLossFactor = 2;
                m_HealthStatusFullLossFactor = 2;
                m_HealthStatusMoraleLossFactor = 2;
                m_HealthStatusText.text = "IS SICK";
                m_HealthStatusText.color = Color.red;
                break;
        }
    }

    public void ArrowDisplay(float newVal, string valueName)
    {
        switch (valueName)
        {
            case "Morale":
                if (newVal > m_oldMoraleValue)
                {
                    if(m_MoraleArrowGreen != null)
                        m_MoraleArrowGreen.Play();
                }
                else
                {
                    if(m_MoraleArrowRed != null)
                        m_MoraleArrowRed.Play();
                }

                m_oldMoraleValue = newVal;
                break;

            case "Full":
                if (newVal > m_oldFullValue)
                {
                    if(m_FullArrowGreen != null)
                        m_FullArrowGreen.Play();
                }
                else
                {
                    if(m_FullArrowRed != null)
                        m_FullArrowRed.Play();
                }

                m_oldFullValue = newVal;
                break;

            case "Warmth":
                if (newVal > m_oldWarmthValue)
                {
                    if(m_WarmthArrowGreen != null)
                        m_WarmthArrowGreen.Play();
                }
                else
                {
                    if(m_WarmthArrowRed != null)
                        m_WarmthArrowRed.Play();
                }

                m_oldWarmthValue = newVal;
                break;
        }
    }

    public void TextColorChanger(float newVal, string valueName)
    {
        switch (valueName)
        {
            case "Morale":
                if (newVal < 3)
                    m_MoraleValueText.color = Color.red;
                else if (newVal < 7)
                    m_MoraleValueText.color = Color.yellow;
                else if (newVal >= 7)
                    m_MoraleValueText.color = Color.green;
                break;

            case "Full":
                if (newVal < 3)
                    m_FullValueText.color = Color.red;
                else if (newVal < 7)
                    m_FullValueText.color = Color.yellow;
                else if (newVal >= 7)
                    m_FullValueText.color = Color.green;
                break;

            case "Warmth":
                if (newVal < 3)
                    m_WarmthValueText.color = Color.red;
                else if (newVal < 7)
                    m_WarmthValueText.color = Color.yellow;
                else if (newVal >= 7)
                    m_WarmthValueText.color = Color.green;
                break;
        }
    }

    public delegate void OnVariableChangeDelegate(float newVal, string valueName);
    public event OnVariableChangeDelegate OnVariableChangeCharacterValues;
}
