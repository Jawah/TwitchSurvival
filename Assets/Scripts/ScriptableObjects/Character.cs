using UnityEngine;

[CreateAssetMenu]
public class Character : ResettableScriptableObject
{
    public string m_CharacterName;

    public Sprite m_CharacterSprite;

    public float m_DailyMoraleLossFactor;
    public float m_DailyFullLossFactor;
    public float m_DailyWarmthLossFactor;

    public float m_FullGainValue;
    public float m_WarmthGainValue;

    public bool m_InUse = false;

    public override void Reset()
    {
        m_InUse = false;
    }
}
