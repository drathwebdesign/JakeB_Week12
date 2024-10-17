using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Potions/PotionEffect")]
public class PotionData : ScriptableObject {
    public string potionName;
    public Color potionColor;
    public enum EffectType { ScaleIncrease, SpeedIncrease, HealthRestore }
    public EffectType effectType;
    public float effectValue;
    public float effectDuration;
}