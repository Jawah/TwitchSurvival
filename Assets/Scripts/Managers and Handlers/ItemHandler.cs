using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    public GameObject m_ItemPanel;
    public GameObject m_ItemPrefab;
    public List<Item> m_AllItems = new List<Item>();
    public List<ItemManager> m_ActiveItems = new List<ItemManager>();

    public void InstantiateNewItem(string itemName)
    {
        foreach (var item in m_AllItems)
        {
            if (item.m_ItemName == itemName && m_ActiveItems.Count < 6)
            {
                var itemSO = item;

                ItemManager tempItem = new ItemManager(itemSO)
                {
                    m_Instance = Instantiate(m_ItemPrefab, m_ItemPanel.transform)
                };

                tempItem.Setup(m_ActiveItems.Count);
                m_ActiveItems.Add(tempItem);
            }
        }
    }
}