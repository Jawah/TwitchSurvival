using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour {
    
    [SerializeField] private GameObject _informationTextPrefab;
    [SerializeField] private GameObject _informationPanel;
    [SerializeField] private GameObject _newItemPrefab;
    [SerializeField] private GameObject _newItemPanel;
    [TextArea]
    [SerializeField] private List<string> _specialTextList = new List<string>();
    
    private enum SpecialEvents { Default, Robbed, SicknessWave, NumberOfTypes };
    private SpecialEvents _specialEvent = SpecialEvents.Default;
    private List<string> _informationPanelTextList = new List<string>();
    private List<Sprite> _newItemsSpriteList = new List<Sprite>();

    [SerializeField] private Sprite foodSprite;
    [SerializeField] private Sprite firewoodSprite;
    [SerializeField] private Sprite medPackSprite;
    
    public void ExecuteInformationWindow()
    {
        ExecuteForDeath();
        ExecuteForSpecials();
        ExecuteForHealth();
        ExecuteForTask();
        ExecuteForHelp();

        foreach (var text in _informationPanelTextList)
        {
            GameObject tempObject = _informationTextPrefab;
            tempObject.GetComponent<TextMeshProUGUI>().text = text;
            Instantiate(tempObject, _informationPanel.transform);
        }
        
        if (!_newItemsSpriteList.Any()) return;
        GameObject instantiatedNewItemPanel = Instantiate(_newItemPanel, _informationPanel.transform);

        foreach (var item in _newItemsSpriteList)
        {
            GameObject tempObject = _newItemPrefab;
            tempObject.GetComponent<Image>().sprite = item;
            Instantiate(tempObject, instantiatedNewItemPanel.transform);
        }
    }

    /*
        IF 2 OF OUR CHARACTER VALUES ARE IN THE RED, HE HAS A 1 IN 3 CHANCE OF DIEING.
        EVERYBODY LOSES 4 MORALE.
    */
    
    private void ExecuteForDeath()
    {
        foreach (var character in GameManager.Instance.characterHandler.activeCharacters.ToList())
        {
            int counter = 0;

            if (character.FullValue < 3)
                counter++;
            if (character.MoraleValue < 3)
                counter++;
            if (character.WarmthValue < 3)
                counter++;

            if(counter >= 2)
            {
                if(Random.Range(0,3) == 0)
                {
                    Destroy(character.Instance);
                    GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                    _informationPanelTextList.Add(
                        character.characterName + " died. Everybody loses Morale. Why keep on going?");

                    foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                    {
                        character2.MoraleValue -= 4;
                    }
                }
            }
        }
    }
    
    /*
        A 1 IN 4 CHANCE OF A RANDOM SPECIAL EVENT OCCURING.
    */

    private void ExecuteForSpecials()
    {
        int somethingHappensRand = Random.Range(0, 4);

        if (somethingHappensRand == 0)
        {
            _specialEvent = (SpecialEvents)Random.Range(1, (int)SpecialEvents.NumberOfTypes);
            Debug.Log(_specialEvent);
        }
        else
        {
            _specialEvent = SpecialEvents.Default;   
        }

        switch (_specialEvent)
        {
            case SpecialEvents.Robbed:
                _informationPanelTextList.Add(_specialTextList[0]);
                GameManager.Instance.FoodValue = 0;
                GameManager.Instance.FirewoodValue = 0;
                GameManager.Instance.MedPackValue = 0;
                break;

            case SpecialEvents.SicknessWave:
                _informationPanelTextList.Add(_specialTextList[1]);
                foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
                {
                    character.healthState = CharacterManager.HealthState.Sick;
                    character.StatusChanger();
                }
                break;
        }
    }

    /*
        A 1 IN 3 CHANCE THE CHARACTER IS CHECKED FOR HIS HEALTH STATUS.
        THEN A 1 IN 3 CHANCE HE GETS SICK IF ONE OF HIS VALUES IS IN THE RED.
    */
    
    private void ExecuteForHealth()
    {
        foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
        {
            int randNum = Random.Range(0, 3);

            if(randNum == 0)
            {
                int randNum2 = Random.Range(0, 3);

                switch (randNum2)
                {
                    case 0:
                        if (character.MoraleValue < 3)
                        {
                            character.healthState = CharacterManager.HealthState.Depressed;
                            character.StatusChanger();
                        }
                        break;

                    case 1:
                        if (character.FullValue < 3)
                        {
                            character.healthState = CharacterManager.HealthState.Fracture;
                            character.StatusChanger();
                        }
                        break;

                    case 2:
                        if (character.WarmthValue < 3)
                        {
                            character.healthState = CharacterManager.HealthState.Sick;
                            character.StatusChanger();
                        }
                        break;
                }
            }

            if (character.healthState != CharacterManager.HealthState.Default)
            {
                if (character.healthState == CharacterManager.HealthState.Depressed)
                {
                    _informationPanelTextList.Add(
                        character.characterName + " is severely depressed.<br>" +
                        character.characterName + " loses Morale faster than the others.");
                }

                else if (character.healthState == CharacterManager.HealthState.Fracture)
                {
                    _informationPanelTextList.Add(
                        character.characterName + " broke a Leg and needs medical attention.<br>" +
                        character.characterName + " starves faster than the others.");
                }

                else if (character.healthState == CharacterManager.HealthState.Sick)
                {
                    _informationPanelTextList.Add(
                        character.characterName + " is sick!");
                }
            }
        }
    }

    private void ExecuteForTask()
    {
        foreach (var character in GameManager.Instance.characterHandler.activeCharacters.ToList())
        {
            switch (character.playerState)
            {
                case CharacterManager.PlayerState.Default:
                    
                    if(Random.Range(0,2) == 0)
                    {
                        _informationPanelTextList.Add(
                        character.characterName + " stayed home and slept surprisingly good and feels re-energized.");
                        character.FullValue += 2;
                        character.MoraleValue += 2;
                        character.WarmthValue += 2;
                    }
                    else if(Random.Range(0,5) == 0)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " stayed home and found some helpful Ressources.");
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(0, 4, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(0, 4, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(0, 2, medPackSprite, ref _newItemsSpriteList);
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " stayed home and nothing happened.");
                    }

                    character.playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.ChopWood:

                    int randNum = Random.Range(0, 10);

                    if (randNum == 0)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " didn't come back. Let's not hope for our friends return.");

                        foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                        {
                            character2.MoraleValue -= 4;
                        }

                        Destroy(character.Instance);
                        GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                        //GameManager.Instance.m_CharacterHandler.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    }
                    else if (randNum == 1)
                    {
                        int randNum2 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " had a lucky day and found: " + GameManager.Instance.itemHandler.allItems[randNum2].itemName);
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum2].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum2].icon);
                    }
                    else if (randNum == 2|| randNum == 3)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brought back a lot of wood today.");
                        GameManager.Instance.FirewoodValue += 5;
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brought back some wood.");
                        GameManager.Instance.FirewoodValue += 2;
                    }

                    character.playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.Plunder:

                    int randNum3 = Random.Range(0, 9);

                    if (randNum3 == 0 || randNum3 == 1)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " didn't come back. Let's not hope for our friends return.");

                        foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                        {
                            character2.MoraleValue -= 4;
                        }

                        Destroy(character.Instance);
                        GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                        //GameManager.Instance.m_CharacterHandler.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    }
                    else if (randNum3 == 2)
                    {
                        int randNum4 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " did loot a lot and brought back " + GameManager.Instance.itemHandler.allItems[randNum4].itemName + " and a lot of ressources.");
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum4].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum4].icon);
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(3, 3, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(2, 2, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(4, 4, medPackSprite, ref _newItemsSpriteList);
                    }
                    else if (randNum3 == 3 || randNum3 == 4)
                    {
                        int randNum5 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " did loot a lot and brought back " + GameManager.Instance.itemHandler.allItems[randNum5].itemName + " and some ressources.");
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum5].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum5].icon);
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(1, 4, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(1, 4, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(1, 4, medPackSprite, ref _newItemsSpriteList);
                    }
                    else if(randNum3 == 5)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " came back barehanded and also broke his leg. That idiot!");
                        character.healthState = CharacterManager.HealthState.Fracture;
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brought back some ressources from this trip.");
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(1, 4, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(1, 4, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(1, 4, medPackSprite, ref _newItemsSpriteList);
                    }

                    character.playerState = CharacterManager.PlayerState.Default;

                    break;
            }
        }
    }

    private void ExecuteForMorale()
    {
        if(Random.Range(0,3) == 0)
        {
            foreach (var character in GameManager.Instance.characterHandler.activeCharacters)
            {
                character.MoraleValue += 3;
                _informationPanelTextList.Add("Morale Boost for all because something good happened.");
            }
        }
    }

    private void ExecuteForHelp()
    {
        if(Random.Range(0, 3) == 0)
        {
            bool characterChosen = false;

            while (!characterChosen)
            {
                Character tempCharacter = GameManager.Instance.characterHandler.allCharacters[Random.Range(0, GameManager.Instance.characterHandler.allCharacters.Count)];
                if (tempCharacter.inUse == false)
                {
                    GameManager.Instance.characterHandler.InstantiateNewCharacter(tempCharacter.name);
                    _informationPanelTextList.Add(tempCharacter.characterName + " joined your House.");
                    characterChosen = true;
                }
            }
        }
    }

    private int AddRandomResourceAmount(int min, int max, Sprite resourceSprite, ref List<Sprite> newItemList)
    {
        int randomResourceAmount = Random.Range(min, max);
        
        if (randomResourceAmount > 0 && !newItemList.Contains(resourceSprite))
            newItemList.Add(resourceSprite);

        return randomResourceAmount;
    }
    
    public void Reset()
    {
        _informationPanelTextList.Clear();
        _newItemsSpriteList.Clear();

        foreach (Transform child in _informationPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}