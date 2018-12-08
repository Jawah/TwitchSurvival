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

    Character m_Character;

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
    }

    public void SetNewCharacterValues(float accumalatedMoraleItemFactors, float accumalatedFullItemFactors, float accumalatedWarmthItemFactors)
    {
        m_MoraleValue += (-m_MoraleLossFactor + accumalatedMoraleItemFactors);
        m_FullValue += (-m_FullLossFactor + accumalatedFullItemFactors);
        m_WarmthValue += (-m_WarmthLossFactor + accumalatedWarmthItemFactors);

        m_MoraleValueText.text = m_MoraleValue.ToString("F1");
        m_FullValueText.text = m_FullValue.ToString("F1");
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
    }

    public void AddFull()
    {
        m_FullValue += m_FullGainValue;
        m_FullValueText.text = m_FullValue.ToString("F1");
    }

    public void AddWarmth()
    {
        m_WarmthValue += m_WarmthGainValue;
        m_WarmthValueText.text = m_WarmthValue.ToString("F1");
    }
}
