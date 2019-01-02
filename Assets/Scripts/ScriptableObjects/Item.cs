using UnityEngine;

[CreateAssetMenu]
public class Item : ResettableScriptableObject
{
    public string m_ItemName;
    public string m_ItemDescription;
    public string m_ItemShortDescription;

    public float m_MoraleFactorChangeValue;
    public float m_FullFactorChangeValue;
    public float m_WarmthFactorChangeValue;

    public Sprite m_Icon;

    public bool m_InUse;

    public override void Reset()
    {
        m_InUse = false;
    }
}