using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject {

    public string m_ItemName;
    public string m_Description;

    public float m_MoraleFactorChangeValue;
    public float m_HungerFactorChangeValue;
    public float m_WarmthFactorChangeValue;

    public Sprite m_Icon;

    public bool m_Breakable;
    public int m_Durability;
}