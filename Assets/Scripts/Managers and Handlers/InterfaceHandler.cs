using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InterfaceHandler : MonoBehaviour {

    public TextMeshProUGUI m_DayText;
    public TextMeshProUGUI m_TemperatureText;
    public TextMeshProUGUI m_CountDownText;
    public TextMeshProUGUI m_FoodText;
    public TextMeshProUGUI m_FirewoodText;
    public TextMeshProUGUI m_MedPackText;
    public TextMeshProUGUI m_QuestionText;

    public GameObject m_AnswersPanel;
    public GameObject m_AnswerPrefab;
    public GameObject m_ResultPanel;
    public GameObject m_SliderPrefab;

    public GameObject m_WinPanel;
    public GameObject m_LosePanel;
    public GameObject m_DayPanel;
    public GameObject m_InformationPanel;

    public void SetCountDownValue(int value)
    {
        m_CountDownText.text = value.ToString();
    }

    public void SetRessourceText()
    {
        m_FoodText.text = GameManager.Instance.FoodValue + "x";
        m_FirewoodText.text = GameManager.Instance.FirewoodValue + "x";
        m_MedPackText.text = GameManager.Instance.MedPackValue + "x";
    }
}
