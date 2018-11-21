using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CharacterManager
{
    public string m_CharacterName;
    public int m_CharacterNumber;

    public GameObject m_Instance;

    public float m_MoraleValue;
    public float m_FullValue;
    public float m_WarmthValue;

    public float m_MoraleLossFactor;
    public float m_FullLossFactor;
    public float m_WarmthLossFactor;

    Character m_Character;

    private TextMeshProUGUI m_NameText;
    private TextMeshProUGUI m_MoraleValueText;
    private TextMeshProUGUI m_FullValueText;
    private TextMeshProUGUI m_WarmthValueText;

    public CharacterManager(Character characterSO)
    {
        m_Character = characterSO;
    }

    public void Setup(int characterNumber)
    {
        m_CharacterNumber = characterNumber;
        m_CharacterName = m_Character.m_CharacterName;
        m_MoraleLossFactor = m_Character.m_DailyMoraleLossFactor;
        m_FullLossFactor = m_Character.m_DailyFullLossFactor;
        m_WarmthLossFactor = m_Character.m_DailyWarmthLossFactor;
        m_MoraleValue = (1 - m_MoraleLossFactor) * Random.Range(5f, 10f);
        m_FullValue = (1 - m_FullLossFactor) * Random.Range(8f, 10f);
        m_WarmthValue = (1 - m_WarmthLossFactor) * Random.Range(5f, 10f);

        var children = m_Instance.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var child in children)
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

        m_NameText.text = m_CharacterName;
        m_MoraleValueText.text = m_MoraleValue.ToString("F1");
        m_FullValueText.text = m_FullValue.ToString("F1");
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
        
    }

    float RoundNumber(float x, int decimalPlaces)
    {
        return Mathf.Round(x * Mathf.Pow(10, decimalPlaces));
    }
}
