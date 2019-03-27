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
        GameManager.Instance.interfaceHandler.scenarioImageHolder.sprite = GameManager.Instance.interfaceHandler.scenarioStandardSprite;
        
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

            if(counter >= 0) // 2
            {
                if(Random.Range(0,1) == 0) //3
                {
                    GameManager.Instance.interfaceHandler.scenarioImageHolder.sprite = GameManager.Instance.interfaceHandler.deathSprite;
                    Destroy(character.Instance);
                    GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                    _informationPanelTextList.Add(
                        character.characterName + " ist verstorben. Alle verlieren Moral. \r\n 'Gibt es noch einen Grund weiterzumachen?'");

                    foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                    {
                        character2.MoraleValue -= 3;
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
        int somethingHappensRand = Random.Range(0, 5);

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
                if (GameManager.Instance.FirewoodValue >= 2)
                    GameManager.Instance.FirewoodValue = 2;
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
            int randNum = Random.Range(0, 2);

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
                        character.characterName + " ist stark depressiv.<br>" +
                        character.characterName + " verliert schneller Moral.");
                }

                else if (character.healthState == CharacterManager.HealthState.Fracture)
                {
                    _informationPanelTextList.Add(
                        character.characterName + " hat ein gebrochenes Bein und benötigt medizinische Behandlung.<br>" +
                        character.characterName + " verhungert schneller als zuvor.");
                }

                else if (character.healthState == CharacterManager.HealthState.Sick)
                {
                    _informationPanelTextList.Add(
                        character.characterName + " hat sich die Grippe eingefangen!");
                }
            }
        }
    }

    private void ExecuteForTask()
    {
        foreach (var character in GameManager.Instance.characterHandler.activeCharacters.ToList())
        {
            int playerStateChance = Random.Range(0, 100);
            switch (character.playerState)
            {
                case CharacterManager.PlayerState.Default:
                    
                    if(playerStateChance >= 60)
                    {
                        _informationPanelTextList.Add(
                        character.characterName + " ist zu Hause geblieben und konnte trotz allem was passiert, etwas Kraft zurückgewinnen.");
                        // character.FullValue += 2;
                        character.MoraleValue += 2;
                        character.WarmthValue += 2;
                    }
                    else if(playerStateChance >= 85)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " ist zu Hause geblieben und hat versteckte, hilfreiche Ressourcen gefunden.");
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(0, 3, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(0, 4, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(0, 2, medPackSprite, ref _newItemsSpriteList);
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " ist zu Hause geblieben.");
                    }

                    character.playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.ChopWood:

                    playerStateChance += character.skillTimber;
                    
                    if (playerStateChance <= 15)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " kam nicht mehr zurück. Lass uns nicht auf die Rückkehr unseres Freunds hoffen.");

                        foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                        {
                            character2.MoraleValue -= 3;
                        }

                        GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                        Destroy(character.Instance);
                        //GameManager.Instance.characterHandler.activeCharacters[character].m_Instance.SetActive(false);
                    }
                    else if (playerStateChance >= 80)
                    {
                        int randNum2 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " hatte eine glückliche Nacht und brachte mit: " + GameManager.Instance.itemHandler.allItems[randNum2].itemName + ".");
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum2].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum2].icon);
                    }
                    else if (playerStateChance >= 60)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brachte eine Menge Feuerholz mit.");
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(3, 5, firewoodSprite, ref _newItemsSpriteList);
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brachte etwas Feuerholz zurück.");
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(1, 3, firewoodSprite, ref _newItemsSpriteList);
                    }

                    character.playerState = CharacterManager.PlayerState.Default;

                    break;

                case CharacterManager.PlayerState.Plunder:

                    playerStateChance += character.skillPlunder;
                    
                    if (playerStateChance <= 25)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " kam nicht mehr zurück. Lass uns nicht auf die Rückkehr unseres Freunds hoffen.");

                        foreach (var character2 in GameManager.Instance.characterHandler.activeCharacters)
                        {
                            character2.MoraleValue -= 3;
                        }

                        GameManager.Instance.characterHandler.activeCharacters.Remove(character);
                        Destroy(character.Instance);
                        //GameManager.Instance.m_CharacterHandler.m_ActiveCharacters[i].m_Instance.SetActive(false);
                    }
                    else if (playerStateChance >= 90)
                    {
                        int randNum4 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " hat sehr viel erbeutet und brachte " + GameManager.Instance.itemHandler.allItems[randNum4].itemName + " und eine Menge Ressourcen zurück.");
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum4].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum4].icon);
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(2, 4, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(2, 4, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(2, 4, medPackSprite, ref _newItemsSpriteList);
                    }
                    else if (playerStateChance >= 75)
                    {
                        int randNum5 = Random.Range(0, GameManager.Instance.itemHandler.allItems.Count);

                        _informationPanelTextList.Add(
                            character.characterName + " hat sehr viel erbeutet und brachte " + GameManager.Instance.itemHandler.allItems[randNum5].itemName + " und etwas an Ressourcen zurück.");
                        GameManager.Instance.itemHandler.InstantiateNewItem(GameManager.Instance.itemHandler.allItems[randNum5].itemName);
                        _newItemsSpriteList.Add(GameManager.Instance.itemHandler.allItems[randNum5].icon);
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(1, 3, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(1, 3, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(1, 3, medPackSprite, ref _newItemsSpriteList);
                    }
                    else if(playerStateChance <= 40)
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " kam mit leeren Händen zurück und brauch sich dabei noch das Bein. Dieser Idiot!");
                        character.healthState = CharacterManager.HealthState.Fracture;
                        character.StatusChanger();
                    }
                    else
                    {
                        _informationPanelTextList.Add(
                            character.characterName + " brachte eine Menge Ressourcen von seinem Ausflug mit.");
                        GameManager.Instance.FoodValue += AddRandomResourceAmount(1, 2, foodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.FirewoodValue += AddRandomResourceAmount(1, 2, firewoodSprite, ref _newItemsSpriteList);
                        GameManager.Instance.MedPackValue += AddRandomResourceAmount(1, 2, medPackSprite, ref _newItemsSpriteList);
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
        if(Random.Range(0, 4) == 0)
        {
            string characterName = GameManager.Instance.characterHandler.RandomInactiveCharacter().characterName;
            GameManager.Instance.characterHandler.InstantiateNewCharacter(characterName);
            _informationPanelTextList.Add(characterName + " ist deinem Haus beigetreten.\r\n'Eine Person weniger, die dort draußen sterben muss!'");
            
            /*
            bool characterChosen = false;

            while (!characterChosen)
            {
                Character tempCharacter = GameManager.Instance.characterHandler.allCharacters[Random.Range(0, GameManager.Instance.characterHandler.allCharacters.Count)];
                if (tempCharacter.inUse == false)
                {
                    GameManager.Instance.characterHandler.InstantiateNewCharacter(tempCharacter.name);
                    _informationPanelTextList.Add(tempCharacter.characterName + " ist deinem Haus beigetreten.\r\n'Eine Person weniger, die dort draußen sterben muss!'");
                    characterChosen = true;
                }
            }
            */
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