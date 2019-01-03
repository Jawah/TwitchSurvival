using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager
{
    public int itemNumber;

    public string itemName;
    public string itemDescription;
    public string itemShortDescription;

    public GameObject Instance;

    public float moraleFactorChangeValue;
    public float fullFactorChangeValue;
    public float warmthFactorChangeValue;

    private Sprite _icon;

    private Item _item;

    private TextMeshProUGUI _shortDescText;
    private Image _image;

    public ItemManager(Item itemSO)
    {
        _item = itemSO;
    }

    public void Setup(int itemNum)
    {
        itemNumber = itemNum;

        itemName = _item.itemName;
        itemDescription = _item.itemDescription;
        itemShortDescription = _item.itemShortDescription;

        moraleFactorChangeValue = _item.moraleFactorChangeValue;
        fullFactorChangeValue = _item.fullFactorChangeValue;
        warmthFactorChangeValue = _item.warmthFactorChangeValue;

        _icon = _item.icon;

        _shortDescText = Instance.GetComponentInChildren<TextMeshProUGUI>();
        _image = Instance.GetComponentInChildren<Image>();

        _shortDescText.text = itemShortDescription;
        _image.sprite = _icon;
    }
}
