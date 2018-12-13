using System.Collections;
using System.Collections.Generic;
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
    public float m_FullValue;
    public float m_WarmthValue;

    public float m_MoraleLossFactor;
    public float m_FullLossFactor;
    public float m_WarmthLossFactor;

    public float m_FullGainValue;
    public float m_WarmthGainValue;

    public bool m_IsPlundering = false;

    public bool m_IsSick = false;

    public enum PlayerState { Default, Plunder, ChopWood };
    public PlayerState playerState = PlayerState.Default;

    private enum HealthState { Default, Sick, Depressed, Fracture };
    private HealthState healthState = HealthState.Default;

    private Character m_Character;

    private TextMeshProUGUI m_NameText;
    private TextMeshProUGUI m_MoraleValueText;
    private TextMeshProUGUI m_FullValueText;
    private TextMeshProUGUI m_WarmthValueText;

    private Image m_CharacterImage;

    public CharacterManager(Character characterSO)
    {
        m_Character = characterSO;
    }

    public void Setup(int characterNumber)
    {
        OnVariableChange += VariableChangeHandler;

        m_CharacterNumber = characterNumber;
        m_CharacterName = m_Character.m_CharacterName;
        m_MoraleLossFactor = m_Character.m_DailyMoraleLossFactor;
        m_FullLossFactor = m_Character.m_DailyFullLossFactor;
        m_CharacterSprite = m_Character.m_CharacterSprite;
        m_WarmthLossFactor = m_Character.m_DailyWarmthLossFactor;
        m_MoraleValue = (1 - m_MoraleLossFactor) * Random.Range(5f, 10f);
        m_FullValue = (1 - m_FullLossFactor) * Random.Range(8f, 10f);
        m_WarmthValue = (1 - m_WarmthLossFactor) * Random.Range(5f, 10f);

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
        }

        var imageChildren = m_Instance.GetComponentsInChildren<Image>();
        foreach (var child in imageChildren)
        {
            if (child.gameObject.CompareTag(Tags.CHARACTER_IMAGE_TAG))
                m_CharacterImage = child.GetComponent<Image>();
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
    }

    public void SetNewCharacterValues(float accumalatedMoraleItemFactors, float accumalatedFullItemFactors, float accumalatedWarmthItemFactors)
    {
        MoraleValue += (-m_MoraleLossFactor + accumalatedMoraleItemFactors);
        FullValue += (-m_FullLossFactor + accumalatedFullItemFactors);
        WarmthValue += (-m_WarmthLossFactor + accumalatedWarmthItemFactors);

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
            if (m_MoraleValue == value) return;
            m_MoraleValue = value;
            if (OnVariableChange != null)
                OnVariableChange(m_MoraleValue, "Morale");
        }
    }

    public float FullValue
    {
        get { return m_FullValue; }
        set
        {
            if (m_FullValue == value) return;
            m_FullValue = value;
            if (OnVariableChange != null)
                OnVariableChange(m_FullValue, "Full");
        }
    }

    public float WarmthValue
    {
        get { return m_WarmthValue; }
        set
        {
            if (m_WarmthValue == value) return;
            m_WarmthValue = value;
            if (OnVariableChange != null)
                OnVariableChange(m_WarmthValue, "Warmth");
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
    public event OnVariableChangeDelegate OnVariableChange;
}
