using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class InterfaceHandler : MonoBehaviour {

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI firewoodText;
    public TextMeshProUGUI medPackText;
    public TextMeshProUGUI questionText;

    public GameObject answersPanel;
    public GameObject answerPrefab;
    public GameObject resultPanel;
    public GameObject sliderPrefab;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject dayPanel;
    public GameObject informationPanel;

    public void SetCountDownValue(int value)
    {
        countDownText.text = value.ToString();
    }

    public void SetRessourceText()
    {
        foodText.text = GameManager.Instance.FoodValue + "x";
        firewoodText.text = GameManager.Instance.FirewoodValue + "x";
        medPackText.text = GameManager.Instance.MedPackValue + "x";
    }
}
