using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionColors", menuName = "AlchemyWorld/PotionColors", order = 2)]
public class ColorSettings : ScriptableObject
{
    public Color LowHealthColor;
    public Color HighHealthColor;

    public Color LowManaColor;
    public Color HighManaColor;

    public Color LowBuffColor;
    public Color HighBuffColor;

    public Color LowDebuffColor;
    public Color HighDebuffColor;

    public Color WaterColor;
}
