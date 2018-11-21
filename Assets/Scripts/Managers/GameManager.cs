using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int m_Day;
    public int m_Temperature;

    public float m_InformationScreenLength;
    public float m_DayStartingLength;
    public float m_DayPlayingLength;
    public float m_DayEndingLength;

    public TextMeshProUGUI m_DayText;
    public TextMeshProUGUI m_TemperatureText;
    public TextMeshProUGUI m_CountDownText;

    public GameObject m_CharacterCardPanel;
    public GameObject m_CharacterCardPrefab;
    public List<Character> m_AllCharacters = new List<Character>();
    public List<CharacterManager> m_ActiveCharacters = new List<CharacterManager>();

    private float m_CountDownValue;
    private int m_CountDownValueInt;
    private int m_CharacterCounter = 0;

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
        #region CountDownTimer
        m_CountDownValue -= Time.deltaTime;
        m_CountDownValueInt = (int)m_CountDownValue;
        m_CountDownText.text = m_CountDownValueInt.ToString();
        #endregion
    }

    public void InstantiateNewCharacter(string characterName)
    {
        Character characterSO = null;

        for (int i = 0; i < m_AllCharacters.Count; i++)
        {
            if (m_AllCharacters[i].m_CharacterName == characterName && m_AllCharacters[i].m_WasUsed == false && m_ActiveCharacters.Count < 6)
            {
                m_AllCharacters[i].m_WasUsed = true;
                characterSO = m_AllCharacters[i];

                CharacterManager tempCharacter = new CharacterManager(characterSO);

                tempCharacter.m_Instance = Instantiate(m_CharacterCardPrefab, m_CharacterCardPanel.transform) as GameObject;
                tempCharacter.Setup(m_CharacterCounter);
                m_ActiveCharacters.Add(tempCharacter);

                m_CharacterCounter++;
            }
        }
    }

    #region SetterFunctions

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

    #endregion

    #region GameLoop

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(InformationScreen());
        yield return StartCoroutine(DayStarting());
        yield return StartCoroutine(DayPlaying());
        yield return StartCoroutine(DayEnding());

        if(m_ActiveCharacters.Count == 0)
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

    #endregion

}
