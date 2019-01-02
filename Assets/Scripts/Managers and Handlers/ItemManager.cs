using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager
{
    public int m_ItemNumber;

    public string m_ItemName;
    public string m_ItemDescription;
    public string m_ItemShortDescription;

    public GameObject m_Instance;

    public float m_MoraleFactorChangeValue;
    public float m_FullFactorChangeValue;
    public float m_WarmthFactorChangeValue;

    private Sprite m_Icon;

    private Item m_Item;

    private TextMeshProUGUI m_ShortDescText;
    private Image m_Image;

    public ItemManager(Item itemSO)
    {
        m_Item = itemSO;
    }

    public void Setup(int itemNumber)
    {
        m_ItemNumber = itemNumber;

        m_ItemName = m_Item.m_ItemName;
        m_ItemDescription = m_Item.m_ItemDescription;
        m_ItemShortDescription = m_Item.m_ItemShortDescription;

        m_MoraleFactorChangeValue = m_Item.m_MoraleFactorChangeValue;
        m_FullFactorChangeValue = m_Item.m_FullFactorChangeValue;
        m_WarmthFactorChangeValue = m_Item.m_WarmthFactorChangeValue;

        m_Icon = m_Item.m_Icon;

        m_ShortDescText = m_Instance.GetComponentInChildren<TextMeshProUGUI>();
        m_Image = m_Instance.GetComponentInChildren<Image>();

        m_ShortDescText.text = m_ItemShortDescription;
        m_Image.sprite = m_Icon;
    }
}
