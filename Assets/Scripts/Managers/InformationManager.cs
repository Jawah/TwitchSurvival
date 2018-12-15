using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationManager : MonoBehaviour {
    
    [SerializeField] GameObject m_InformationTextPrefab;
    [SerializeField] GameObject m_InformationPanel;
    [TextArea]
    [SerializeField] List<string> m_SpecialTextList = new List<string>();

    private enum SpecialEvents { Default, Robbed, SicknessWave, NumberOfTypes };
    private SpecialEvents specialEvent = SpecialEvents.Default;
    private List<string> m_InformationPanelTextList = new List<string>();

    public void ExecuteInformationWindow()
    {
        ExecuteForSpecials();
        ExecuteForHealth();
        ExecuteForTask();

        for(int i = 0; i < m_InformationPanelTextList.Count; i++)
        {
            GameObject tempObject = m_InformationTextPrefab;
            tempObject.GetComponent<TextMeshProUGUI>().text = m_InformationPanelTextList[i];
            Instantiate(tempObject, m_InformationPanel.transform);
        }
    }

    void ExecuteForSpecials()
    {
        int somethingHappensRand = Random.Range(0, 5);

        if (somethingHappensRand == 0)
        {
            specialEvent = (SpecialEvents)Random.Range(1, (int)SpecialEvents.NumberOfTypes);
            Debug.Log(specialEvent);
        }
        else
        {
            specialEvent = SpecialEvents.Default;   
        }

        switch (specialEvent)
        {
            case SpecialEvents.Robbed:
                m_InformationPanelTextList.Add(m_SpecialTextList[0]);
                break;

            case SpecialEvents.SicknessWave:
                m_InformationPanelTextList.Add(m_SpecialTextList[1]);
                break;
        }
    }

    void ExecuteForHealth()
    {
        for (int i = 0; i < GameManager.Instance.m_ActiveCharacters.Count; i++)
        {
            int randNum = Random.Range(0, 3);

            if(randNum == 0)
            {
                int randNum2 = Random.Range(0, 3);

                switch (randNum2)
                {
                    case 0:
                        if (GameManager.Instance.m_ActiveCharacters[i].MoraleValue < 3)
                        {
                            GameManager.Instance.m_ActiveCharacters[i].healthState = CharacterManager.HealthState.Depressed;
                            GameManager.Instance.m_ActiveCharacters[i].StatusChanger();
                        }
                        break;

                    case 1:
                        if (GameManager.Instance.m_ActiveCharacters[i].FullValue < 3)
                        {
                            GameManager.Instance.m_ActiveCharacters[i].healthState = CharacterManager.HealthState.Fracture;
                            GameManager.Instance.m_ActiveCharacters[i].StatusChanger();
                        }
                        break;

                    case 2:
                        if (GameManager.Instance.m_ActiveCharacters[i].WarmthValue < 3)
                        {
                            GameManager.Instance.m_ActiveCharacters[i].healthState = CharacterManager.HealthState.Sick;
                            GameManager.Instance.m_ActiveCharacters[i].StatusChanger();
                        }
                        break;
                }
            }

            if (GameManager.Instance.m_ActiveCharacters[i].healthState != CharacterManager.HealthState.Default)
            {
                if (GameManager.Instance.m_ActiveCharacters[i].healthState == CharacterManager.HealthState.Depressed)
                {
                    m_InformationPanelTextList.Add(
                            GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + "is severly depressed.<br>" +
                            GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " loses Morale faster than the others.");
                }

                else if (GameManager.Instance.m_ActiveCharacters[i].healthState == CharacterManager.HealthState.Fracture)
                {
                    m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + "broke a Leg and needs medical attention.<br>" +
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " starves faster than the others.");
                }

                else if (GameManager.Instance.m_ActiveCharacters[i].healthState == CharacterManager.HealthState.Sick)
                {
                    m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + "got sick because she's freezing.<br>" +
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + "Take care of the temperature in the house.");
                }
            }
        }
    }

    void ExecuteForTask()
    {
        for (int i = 0; i < GameManager.Instance.m_ActiveCharacters.Count; i++)
        {
            switch (GameManager.Instance.m_ActiveCharacters[i].playerState)
            {
                case CharacterManager.PlayerState.Default:

                    break;

                case CharacterManager.PlayerState.ChopWood:

                    break;

                case CharacterManager.PlayerState.Plunder:

                    break;
            }
        }
    }

    public void Reset()
    {
        m_InformationPanelTextList.Clear();

        foreach (Transform child in m_InformationPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
