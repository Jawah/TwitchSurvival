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
        ExecuteForDeath();
        ExecuteForSpecials();
        ExecuteForHealth();
        ExecuteForTask();
        ExecuteForHelp();

        for(int i = 0; i < m_InformationPanelTextList.Count; i++)
        {
            GameObject tempObject = m_InformationTextPrefab;
            tempObject.GetComponent<TextMeshProUGUI>().text = m_InformationPanelTextList[i];
            Instantiate(tempObject, m_InformationPanel.transform);
        }
    }

    void ExecuteForDeath()
    {
        for(int i = 0; i < GameManager.Instance.m_ActiveCharacters.Count; i++)
        {
            int counter = 0;

            if (GameManager.Instance.m_ActiveCharacters[i].FullValue < 3)
                counter++;
            if (GameManager.Instance.m_ActiveCharacters[i].MoraleValue < 3)
                counter++;
            if (GameManager.Instance.m_ActiveCharacters[i].WarmthValue < 3)
                counter++;

            if(counter >= 2)
            {
                if(Random.Range(0,3) == 0)
                {
                    Destroy(GameManager.Instance.m_ActiveCharacters[i].m_Instance);
                    //GameManager.Instance.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " died. Everbody loses Morale. Why keep on going?");

                    for (int j = 0; j < GameManager.Instance.m_ActiveCharacters.Count; j++)
                    {
                        GameManager.Instance.m_ActiveCharacters[j].MoraleValue -= 4;
                    }
                }
            }
        }
    }

    void ExecuteForSpecials()
    {
        int somethingHappensRand = Random.Range(0, 4);

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
                GameManager.Instance.FoodValue = 0;
                GameManager.Instance.FirewoodValue = 0;
                GameManager.Instance.MedPackValue = 0;
                break;

            case SpecialEvents.SicknessWave:
                m_InformationPanelTextList.Add(m_SpecialTextList[1]);
                for(int i = 0; i < GameManager.Instance.m_ActiveCharacters.Count; i++)
                {
                    GameManager.Instance.m_ActiveCharacters[i].healthState = CharacterManager.HealthState.Sick;
                    GameManager.Instance.m_ActiveCharacters[i].StatusChanger();
                }
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
                            GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " is severly depressed.<br>" +
                            GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " loses Morale faster than the others.");
                }

                else if (GameManager.Instance.m_ActiveCharacters[i].healthState == CharacterManager.HealthState.Fracture)
                {
                    m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " broke a Leg and needs medical attention.<br>" +
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " starves faster than the others.");
                }

                else if (GameManager.Instance.m_ActiveCharacters[i].healthState == CharacterManager.HealthState.Sick)
                {
                    m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " is sick!");
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

                    if(Random.Range(0,2) == 0)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " stayed home and slept surprisingly good and feels re-energized.");
                        GameManager.Instance.m_ActiveCharacters[i].FullValue += 2;
                        GameManager.Instance.m_ActiveCharacters[i].MoraleValue += 2;
                        GameManager.Instance.m_ActiveCharacters[i].WarmthValue += 2;
                    }
                    else if(Random.Range(0,5) == 0)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " stayed home and found some helpful Ressources.");
                        GameManager.Instance.FoodValue += Random.Range(0, 4);
                        GameManager.Instance.FirewoodValue += Random.Range(0, 4);
                        GameManager.Instance.MedPackValue += Random.Range(0, 2);
                    }
                    else
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " stayed home and nothing happened.");
                    }

                    GameManager.Instance.m_ActiveCharacters[i].playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.ChopWood:

                    int randNum = Random.Range(0, 10);

                    if (randNum == 0)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " didn't come back. Let's not hope for our friends return.");

                        for (int j = 0; j < GameManager.Instance.m_ActiveCharacters.Count; j++)
                        {
                            GameManager.Instance.m_ActiveCharacters[j].MoraleValue -= 4;
                        }

                        Destroy(GameManager.Instance.m_ActiveCharacters[i].m_Instance);
                        //GameManager.Instance.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    }
                    else if (randNum == 1)
                    {
                        int randNum2 = Random.Range(0, GameManager.Instance.m_AllItems.Count);

                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " had a lucky day and found: " + GameManager.Instance.m_AllItems[randNum2].m_ItemName);
                        GameManager.Instance.InstantiateNewItem(GameManager.Instance.m_AllItems[randNum2].m_ItemName);
                    }
                    else if (randNum == 2|| randNum == 3)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " brought back a lot of wood today.");
                        GameManager.Instance.m_FoodValue += 5;
                    }
                    else
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " brought back some wood.");
                        GameManager.Instance.m_FoodValue += 2;
                    }

                    GameManager.Instance.m_ActiveCharacters[i].playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.Plunder:

                    int randNum3 = Random.Range(0, 10);

                    if (randNum3 == 0 || randNum3 == 1)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " didn't come back. Let's not hope for our friends return.");

                        for (int j = 0; j < GameManager.Instance.m_ActiveCharacters.Count; j++)
                        {
                            GameManager.Instance.m_ActiveCharacters[j].MoraleValue -= 4;
                        }

                        Destroy(GameManager.Instance.m_ActiveCharacters[i].m_Instance);
                        //GameManager.Instance.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    }
                    else if (randNum3 == 2)
                    {
                        int randNum4 = Random.Range(0, GameManager.Instance.m_AllItems.Count);

                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " did loot a lot and brought back " + GameManager.Instance.m_AllItems[randNum4].m_ItemName + "and a lot of ressources.");
                        GameManager.Instance.InstantiateNewItem(GameManager.Instance.m_AllItems[randNum4].m_ItemName);
                        GameManager.Instance.FoodValue += 3;
                        GameManager.Instance.FirewoodValue += 2;
                        GameManager.Instance.MedPackValue += 4;
                    }
                    else if (randNum3 == 3 || randNum3 == 4)
                    {
                        int randNum5 = Random.Range(0, GameManager.Instance.m_AllItems.Count);

                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " did loot a lot and brought back " + GameManager.Instance.m_AllItems[randNum5].m_ItemName + "and some ressources.");
                        GameManager.Instance.InstantiateNewItem(GameManager.Instance.m_AllItems[randNum5].m_ItemName);
                        GameManager.Instance.FoodValue += Random.Range(1, 4);
                        GameManager.Instance.FirewoodValue += Random.Range(1, 4);
                        GameManager.Instance.MedPackValue += Random.Range(1, 4);
                    }
                    else if(randNum3 == 5)
                    {
                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " came back barehanded and also broke his leg. That idiot!");
                        GameManager.Instance.m_ActiveCharacters[i].healthState = CharacterManager.HealthState.Fracture;
                    }
                    else
                    {
                        int randNum5 = Random.Range(0, GameManager.Instance.m_AllItems.Count);

                        m_InformationPanelTextList.Add(
                        GameManager.Instance.m_ActiveCharacters[i].m_CharacterName + " brought back some ressources from this trip.");
                        GameManager.Instance.FoodValue += Random.Range(1, 4);
                        GameManager.Instance.FirewoodValue += Random.Range(1, 4);
                        GameManager.Instance.MedPackValue += Random.Range(1, 4);
                    }

                    GameManager.Instance.m_ActiveCharacters[i].playerState = CharacterManager.PlayerState.Default;

                    break;
            }
        }
    }

    void ExecuteForHelp()
    {
        if(Random.Range(0, 12) == 0)
        {
            bool characterChosen = false;

            while (!characterChosen)
            {
                Character tempCharacter = GameManager.Instance.m_AllCharacters[Random.Range(0, GameManager.Instance.m_AllCharacters.Count)];
                if (tempCharacter.m_InUse == false)
                {
                    GameManager.Instance.InstantiateNewCharacter(tempCharacter.name);
                    m_InformationPanelTextList.Add(tempCharacter.m_CharacterName + " joined your House.");
                    characterChosen = true;
                }
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