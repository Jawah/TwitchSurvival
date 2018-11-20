using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {

    public int m_Day;
    public int m_Temperature;

    public float m_InformationScreenLength;
    public float m_DayStartingLength;
    public float m_DayPlayingLength;
    public float m_DayEndingLength;

    public TextMeshProUGUI m_DayText;
    public TextMeshProUGUI m_TemperatureText;
    public TextMeshProUGUI m_CountDownText;

    private int m_CharacterNumber = 2;

    private float m_CountDownValue;
    private int m_CountDownValueInt;

    private WaitForSeconds m_WaitForInformationScreen;
    private WaitForSeconds m_WaitForDayStarting;
    private WaitForSeconds m_WaitForDayPlaying;
    private WaitForSeconds m_WaitForDayEnding;

    private void Awake()
    {
        m_WaitForInformationScreen = new WaitForSeconds(m_InformationScreenLength);
        m_WaitForDayStarting = new WaitForSeconds(m_DayStartingLength);
        m_WaitForDayPlaying = new WaitForSeconds(m_DayPlayingLength);
        m_WaitForDayEnding = new WaitForSeconds(m_DayEndingLength);
    }

    void Start()
    {
        SetDay();
        SetTemperature();
        SetRessources();
        SetItems();
        SetCharacters();

        StartCoroutine(GameLoop());
	}

    private void Update()
    {
        m_CountDownValue -= Time.deltaTime;

        m_CountDownValueInt = (int)m_CountDownValue;
        m_CountDownText.text = m_CountDownValueInt.ToString();
    }

    private void SetDay()
    {
        m_DayText.text = "DAY " + m_Day;
    }

    private void SetTemperature()
    {
        m_TemperatureText.text = m_Temperature + "°C";
    }

    private void SetRessources()
    {

    }

    private void SetItems()
    {

    }

    private void SetCharacters()
    {

    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(InformationScreen());
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());
        yield return StartCoroutine(DayEnding());

        if(m_CharacterNumber == 0)
        {
            // END THE GAME
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator InformationScreen()
    {
        m_CountDownValue = m_InformationScreenLength;

        //BIG EVENT CHECK
        yield return m_WaitForInformationScreen;
    }

    private IEnumerator DayStarting()
    {
        m_CountDownValue = m_DayStartingLength;

        //Add new Items to List if gotten
        yield return m_WaitForDayStarting;
    }

    private IEnumerator DayPlaying()
    {
        m_CountDownValue = m_DayPlayingLength;

        yield return m_WaitForDayPlaying;
    }

    private IEnumerator DayEnding()
    {
        m_CountDownValue = m_DayEndingLength;

        yield return m_WaitForDayEnding;
    }
}
