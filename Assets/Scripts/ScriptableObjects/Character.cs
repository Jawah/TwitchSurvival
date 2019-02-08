using UnityEngine;

[CreateAssetMenu]
public class Character : ResettableScriptableObject
{
    public string characterName;

    public Sprite characterSprite;

    [Range(-25, 25)]public int skillTimber;
    [Range(-25, 25)]public int skillPlunder;
    
    public float dailyMoraleLossFactor;
    public float dailyFullLossFactor;
    public float dailyWarmthLossFactor;

    public float fullGainValue;
    public float warmthGainValue;

    public bool inUse;

    public override void Reset()
    {
        inUse = false;
    }
}
