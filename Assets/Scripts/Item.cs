using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    INGREDIENT,
    POTION,
    POWDER
}

public enum Quality
{
    F,  //0
    E,  //15
    D,  //30
    C,  //45
    B,  //60
    A,  //75
    S  //90
}

public enum DropQuality
{
    LOW,
    MID,
    HIGH,
    ALL
}

[Serializable]
public class Item
{
    protected ItemType _itemType = ItemType.INGREDIENT;
    [SerializeField] protected Effect[] _effects = new Effect[] { };
    protected int _qualityValue = 0;
    private Quality _quality = Quality.F;
    [SerializeField] protected string _name = "???";

    protected int _lowTreshold = 30;
    protected int _midTreshold = 75;
    protected int _highTreshold = 100;

    private int _fMax = 15;
    private int _eMax = 30;
    private int _dMax = 45;
    private int _cMax = 60;
    private int _bMax = 75;
    private int _aMax = 90;

    public ItemType ItemType { get { return _itemType; } }
    public Effect[] Effects { get { return _effects; } }
    public Quality Quality { get { return _quality; } }
    public int QualityValue { get { return _qualityValue; } }
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    protected void InitializeQuality()
    {
        if (_qualityValue <= _fMax) _quality = Quality.F;
        else if (_qualityValue <= _eMax) _quality = Quality.E;
        else if (_qualityValue <= _dMax) _quality = Quality.D;
        else if (_qualityValue <= _cMax) _quality = Quality.C;
        else if (_qualityValue <= _bMax) _quality = Quality.B;
        else if (_qualityValue <= _aMax) _quality = Quality.A;
        else _quality = Quality.S;
    }

    protected void InitializeQuality(int quality)
    {
        _qualityValue= quality;
        InitializeQuality();
    }

    public void SetDropQuality(DropQuality dropQuality)
    {
        switch (dropQuality)
        {
            case DropQuality.LOW:
                _qualityValue = UnityEngine.Random.Range(0, _lowTreshold + 1);
                break;
            case DropQuality.MID:
                _qualityValue = UnityEngine.Random.Range(_lowTreshold + 1, _midTreshold + 1);
                break;
            case DropQuality.HIGH:
                _qualityValue = UnityEngine.Random.Range(_midTreshold + 1, _highTreshold + 1);
                break;
            case DropQuality.ALL:
                _qualityValue = UnityEngine.Random.Range(0, _highTreshold + 1);
                break;
            default:
                break;
        }

        InitializeQuality();
    }
}

[Serializable]
public class IngredientItem : Item
{
    public void SetEffects(Effect[] effects)
    {
        _effects = effects;
    }
}

[Serializable]
public class PotionItem : IngredientItem
{
    public Color LiquidColor { get; private set; } = Color.white;
    public int Capacity { get; private set; }
    public uint Id { get; private set; } = 0;
    public uint Price { get; set; } = 10;
    public PotionItem()
    {
        _itemType = ItemType.POTION;
    }

    public void SetColor(Color color)
    {
        LiquidColor = color;
    }
    public void SetCapacity(int capacity)
    {
        Capacity = capacity;
    }

    public void SetQuality(int quality)
    {
        InitializeQuality(quality);
    }

    public void SetId(uint id)
    {
        Id = id;
    }
}

public class PowderItem : IngredientItem
{
    public PowderItem()
    {
        _itemType = ItemType.POWDER;
    }
}
