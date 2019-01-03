using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemHandler : MonoBehaviour {

    public GameObject itemPanel;
    public GameObject itemPrefab;
    public List<Item> allItems = new List<Item>();
    public List<ItemManager> activeItems = new List<ItemManager>();

    public void InstantiateNewItem(string itemName)
    {
        foreach (var item in allItems)
        {
            if (item.itemName == itemName && activeItems.Count < 6)
            {
                var itemSO = item;

                ItemManager tempItem = new ItemManager(itemSO)
                {
                    Instance = Instantiate(itemPrefab, itemPanel.transform)
                };

                tempItem.Setup(activeItems.Count);
                activeItems.Add(tempItem);
            }
        }
    }
}