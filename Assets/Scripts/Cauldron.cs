using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [Header("Default Parameters")]
    [SerializeField] private Transform _StickBase;
    [SerializeField] public Transform _WaterSurface;
    [SerializeField] private Transform _StickTop;
    [SerializeField] private ParticleSystem _Ripple;
    [SerializeField] private ColorSettings _ColorSettings;

    [Header("Test Parameters")]
    [SerializeField] public float _Radius;
    [SerializeField] private int _NrOfEdges;
    [SerializeField] private float _StickMass;
    [SerializeField] private int _NrOfRotations = 3;

    [Header("Materials")]
    [SerializeField] private MeshRenderer _Water;

    [Header("UI")]
    [SerializeField] private Slider _ProgressBar;
    [SerializeField] private TMP_Text _ProgressText;

    [Header("Sound")]
    [SerializeField] private EventReference _StickAgainstCauldron;
    [SerializeField] private EventReference _WaterImpactSound;

    private CustomInteraction _interaction = null;
    public CustomInteraction Interaction { set { if (value == null && _interaction != null) _isStickResting = false; _interaction = value; } }

    private float _gravity = 9.81f;

    private bool _isStickResting = false;

    private Vector3 _pointOnCircle;
    private Vector3 _oldPointOnCircle;
    private Vector3 _pointRightDirection;
    private float _length;
    private float _startAngle;
    private float _maxAngle;
    private float _omega = 0.0f;
    private float _totalBlendDistance;
    private float _currentBlendDistance = 0;

    private List<Ingredient> _enteredIngredients= new List<Ingredient>();
    private List<KeyValuePair<PotionItem, int>> _enteredPotions = new List<KeyValuePair<PotionItem, int>>();
    private List<Effect> _effects = new List<Effect>();

    private List<KeyValuePair<PotionItem, int>> _totalEnteredPotions = new List<KeyValuePair<PotionItem, int>>();

    private Color _currentColor = Color.white;
    private Color _currentBaseColor = Color.white;
    private Color _enteredColor;
    private float _lightColorDifference = 50f;
    private bool _isFirstBlend = true;
    private bool _isBlending = false;

    private int _totalQualityValue = 0;
    private int _nrOfEnteredNaturalIngredients = 0;
    private float _totalCombinedBlending = 0f;
    private int _maxBlending = 0;

    private float _blendPercentage;

    private Gradient _firstBlend = new Gradient();
    private GradientColorKey[] _firstBlendKeys = new GradientColorKey[2];
    private GradientAlphaKey[] _firstBlendAlphaKeys = new GradientAlphaKey[2];

    private void Initialize()
    {
        _firstBlendKeys[0].time = 0.0f;
        _firstBlendKeys[1].time = 1.0f;

        _firstBlendAlphaKeys[0].alpha = 1.0f;
        _firstBlendAlphaKeys[0].time = 0.0f;
        _firstBlendAlphaKeys[1].alpha = 1.0f;
        _firstBlendAlphaKeys[1].time = 1.0f;

        FindPointOnCircle(ref _pointOnCircle, _StickBase.up);
        _oldPointOnCircle = _pointOnCircle;
        FindClosestPointOnCircle(_StickBase.up);
        _length = (_StickTop.position - _StickBase.position).magnitude;
        _maxAngle = Vector3.Angle(Vector3.up, ((_WaterSurface.position + Vector3.forward * _Radius) - _StickBase.position).normalized);

        //circumference
        _totalBlendDistance = (2 * Mathf.PI * _Radius) * _NrOfRotations;

        ResetWater();
    }

    public void ResetWater()
    {
        _Water.material.SetColor("_WaterColorDark", _ColorSettings.WaterColor);
        var lightColor = AddLightColorToWater(_ColorSettings.WaterColor);
        _Water.material.SetColor("_WaterColor", lightColor);
        _firstBlendKeys[0].color = _ColorSettings.WaterColor;
        _firstBlendKeys[1].color = _ColorSettings.WaterColor;
        _effects = new List<Effect>();
        _totalQualityValue = 0;
        _nrOfEnteredNaturalIngredients = 0;
        _totalEnteredPotions.Clear();
        _maxBlending = 0;
        _blendPercentage = 0.0f;
        _totalCombinedBlending= 0;
        UpdateUI();
    }
    private void Awake()
    {
        Initialize();
    }
    private void FixedUpdate()
    {
        if (_interaction == null && !_isStickResting)
        {
            ResetStickLocation();
            FindPointOnCircle(ref _pointOnCircle, _StickBase.up);
            UpdateRipple();
            _oldPointOnCircle = _pointOnCircle;
        }
    }

    private void Update()
    {
        if (_interaction != null)
        {
            if ((_interaction.transform.position - _StickBase.position).magnitude > _length)
            {
                _interaction.ReleaseStick();
                _interaction = null;
                _isStickResting = false;
                FindClosestPointOnCircle(_StickBase.up);
            }
            else
            {
                UpdateStickLocation();
                FindPointOnCircle(ref _pointOnCircle, _StickBase.up);
                if (_isBlending)
                    UpdateBlend();
                UpdateRipple();
                UpdateUI();
            }

        }

        _oldPointOnCircle = _pointOnCircle;
    }

    private Color AddLightColorToWater(Color darkColor)
    {
        Color lightColor = (darkColor + new Color(_lightColorDifference / 255f, _lightColorDifference / 255f, _lightColorDifference / 255f));
        lightColor.r = Mathf.Clamp01(lightColor.r);
        lightColor.g = Mathf.Clamp01(lightColor.g);
        lightColor.b = Mathf.Clamp01(lightColor.b);
        return lightColor;
    }

    private void UpdateRipple()
    {
        _Ripple.transform.position = _pointOnCircle;
    }

    private void UpdateUI()
    {
        _ProgressBar.value = _blendPercentage;
        _ProgressText.text = ((int)(_blendPercentage * 100f)).ToString() + "%";
    }

    //How much has the player blended the new materials
    private void UpdateBlend()
    {
        if (_currentBlendDistance >= _totalBlendDistance)
            return;

        var nextDistance = (_pointOnCircle - _oldPointOnCircle).magnitude;
        _currentBlendDistance += nextDistance;

        if (nextDistance > 0)
        {
            _blendPercentage = Mathf.Clamp01(_currentBlendDistance / _totalBlendDistance);
            if (_blendPercentage == 1f)
                _isBlending= false;
            if (_enteredIngredients.Count > 0
                || _enteredPotions.Count > 0)
                AllocateEnteredIngredient();
            UpdateColor();
        }
    }

    private void AllocateEnteredIngredient()
    {
        AllocateEffects();
        GetBlendColorEnteredIngredients();
        if (_isFirstBlend)
        {
            _firstBlendKeys[1].color = _enteredColor;
            _firstBlend.SetKeys(_firstBlendKeys, _firstBlendAlphaKeys);
        }
        _enteredIngredients = new List<Ingredient>();
        _enteredPotions = new List<KeyValuePair<PotionItem, int>>();
    }

    private void AllocateEffects()
    {
        foreach (var ingredient in _enteredIngredients)
        {
            _nrOfEnteredNaturalIngredients++;
            _totalQualityValue += ingredient.Item.QualityValue;
            _effects.AddRange(ingredient.Item.Effects);
        }
        foreach (var potion in _enteredPotions)
        {
            _effects.AddRange(potion.Key.Effects);
            if (_totalEnteredPotions.Count > 0)
            {
                bool isnew = true;
                for (int i = 0; i < _totalEnteredPotions.Count; i++)
                {
                    if (_totalEnteredPotions[i].Key.Id == potion.Key.Id)
                    {
                        isnew = false;
                        if (_totalEnteredPotions[i].Key.Quality == potion.Key.Quality)
                        {
                            _totalEnteredPotions[i] = new KeyValuePair<PotionItem, int>(_totalEnteredPotions[i].Key, _totalEnteredPotions[i].Value + potion.Value);
                        }
                        else _totalEnteredPotions.Add(new KeyValuePair<PotionItem, int>(potion.Key, potion.Value));
                    }
                }
                if (isnew) _totalEnteredPotions.Add(new KeyValuePair<PotionItem, int>(potion.Key, potion.Value));
            }
            else _totalEnteredPotions.Add(new KeyValuePair<PotionItem, int>(potion.Key, potion.Value));
        }
    }

    public Color GetColorWater()
    {
        return _currentColor;
    }
    public Effect[] GetEffects()
    {
        var effects = GetFilteredEffects();
        _effects = new List<Effect>();
        _isFirstBlend = true;
        _isBlending= false;
        _currentBlendDistance= 0;
        return effects;
    }

    //removes double effects
    private Effect[] GetFilteredEffects()
    {
        List<Effect> filteredEffects = new List<Effect>();
        while (_effects.Count > 0)
        {
            var effects = _effects.FindAll(x => x.Equals(_effects[0]));
            filteredEffects.Add(effects[0]);
            _effects.RemoveAll(x => x.Equals(_effects[0]));
        }
        return filteredEffects.ToArray();
    }

    public int GetQualityValue()
    {
        int totalIngredientsUsed = 0;
        for (int i = 0; i < _totalEnteredPotions.Count; i++)
        {
            float amountOfPotion = (float)_totalEnteredPotions[i].Value / 100f;

            int extraPotion = 0;
            if (amountOfPotion % 1 > 0) extraPotion = 1;
            int amountOfPotionINT = (int)amountOfPotion + extraPotion;

            totalIngredientsUsed += amountOfPotionINT;
            _totalQualityValue += (int)(_totalEnteredPotions[i].Key.QualityValue * amountOfPotion);
        }

        Debug.Log("Base Quality Value: " + _totalQualityValue);
        totalIngredientsUsed += _nrOfEnteredNaturalIngredients;
        float blendQuality = (_blendPercentage + _totalCombinedBlending) / (float)_maxBlending;
        Debug.Log("Blend Percentage: " + _blendPercentage);
        Debug.Log("Total Blend Percentage: " + _totalCombinedBlending);
        Debug.Log("Max Blend Percentage: " + _maxBlending);
        Debug.Log("Entered Ingredients: " + totalIngredientsUsed);
        int qualityValue = (int)((_totalQualityValue / totalIngredientsUsed) * blendQuality);
        return qualityValue;
    }
    //how should the current water color look like
    private void UpdateColor()
    {
        if (_isFirstBlend)
        {
            _currentColor = _firstBlend.Evaluate(_blendPercentage);
        }
        else
        {
            Color tempBase = _currentBaseColor;
            FindBlendColor(ref tempBase, _enteredColor, _blendPercentage);
            _currentColor = tempBase;
        }
        _Water.material.SetColor("_WaterColorDark", _currentColor);
        var lightColor = AddLightColorToWater(_currentColor);
        _Water.material.SetColor("_WaterColor", lightColor);
    }

    private void GetBlendColorEnteredIngredients()
    {
        Color tempColor = Color.white;
        bool firstColor = true;
        foreach (var item in _enteredIngredients)
        {
            foreach (var effect in item.Item.Effects)
            {
                switch (effect.Type)
                {
                    case EffectType.HEALTH:
                        FindBlendColor(ref tempColor, _ColorSettings.HighHealthColor, ref firstColor);
                        break;
                    case EffectType.MANA:
                        FindBlendColor(ref tempColor, _ColorSettings.HighManaColor, ref firstColor);
                        break;
                    case EffectType.BUFF:
                        FindBlendColor(ref tempColor, _ColorSettings.HighBuffColor, ref firstColor);
                        break;
                    case EffectType.DEBUFF:
                        FindBlendColor(ref tempColor, _ColorSettings.HighDebuffColor, ref firstColor);
                        break;
                    default:
                        break;
                }
            }
        }
        foreach (var potion in _enteredPotions)
        {
            FindPotionBlendColor(ref tempColor, potion.Key.LiquidColor, potion.Value / 100, ref firstColor);
        }
        _enteredColor= tempColor;
    }

    private void FindPotionBlendColor(ref Color blendColor, Color newColor, float amountPercentage, ref bool firstColor)
    {
        Color potionColor = Color.Lerp(Color.white, newColor, amountPercentage);
        if (firstColor)
        {
            blendColor = potionColor;
            firstColor = false;
        }
        else
            FindBlendColor(ref blendColor, potionColor);
    }
    private void FindBlendColor(ref Color blendColor, Color newColor, float blendPercentage = 1f)
    {
        //blend is the midpoint between 2 colors
        blendPercentage /= 2;
        blendColor = Color.Lerp(blendColor, newColor, blendPercentage);
    }
    private void FindBlendColor(ref Color blendColor, Color newColor, ref bool firstColor)
    {
        if (firstColor)
        {
            blendColor = newColor;
            firstColor = false;
        }
        else
        {
            FindBlendColor(ref blendColor, newColor);
        }
    }

    private void ResetStickLocation()
    {
        float deltaTime = Time.fixedDeltaTime;
        float alpha = 3 * Mathf.Sin(Mathf.Deg2Rad * _startAngle) * ((_StickMass * _gravity) / (2 * _length));
        _omega += alpha * deltaTime;
        float deltaAngle = _omega * deltaTime;
        
        _startAngle += Mathf.Rad2Deg * (_omega * deltaTime);
        if (_startAngle > _maxAngle)
        {
            Vector3 pointer = (_pointOnCircle - _StickBase.position).normalized;
            float angle = Vector3.SignedAngle(_StickBase.up, pointer, _pointRightDirection);
            _StickBase.Rotate(_pointRightDirection, angle);
            _omega = 0.0f;
            _isStickResting = true;

            //play sound
            RuntimeManager.PlayOneShot(_StickAgainstCauldron, _pointOnCircle);
        }
        else
            _StickBase.Rotate(_pointRightDirection, Mathf.Rad2Deg * deltaAngle);
    }

    private void UpdateStickLocation()
    {
        Vector3 newVector = (_interaction.transform.position - _StickBase.position).normalized;
        float angle = Vector3.Angle(Vector3.up, newVector);
        if (angle > _maxAngle)
        {
            FindClosestPointOnCircle(newVector);
            _StickBase.up = (_pointOnCircle - _StickBase.position).normalized;
        }
        else
        {
            _StickBase.up = newVector;
        }
    }

    private void FindPointOnCircle(ref Vector3 pointInCircle, Vector3 up)
    {
        Vector3 vectorToCenter = _WaterSurface.position - _StickBase.position;
        //find the hypothenusa
        //get angle
        float angle = Vector3.Angle(Vector3.up, up);
        _startAngle = angle;
        //get opposite
        float side = vectorToCenter.magnitude;
        float opposite = side * Mathf.Tan(Mathf.Deg2Rad * angle);

        //get hypothenuza
        float hypothenusa = Mathf.Sqrt(Mathf.Pow(opposite, 2) + Mathf.Pow(side, 2));

        //get 3d point of point in circle
        pointInCircle = _StickBase.up * hypothenusa + _StickBase.position;
    }

    private void FindClosestPointOnCircle(Vector3 up)
    {
        Vector3 pointInCircle = new Vector3();
        FindPointOnCircle(ref pointInCircle, up);

        //A = center of circle
        //B = point
        //r = radius
        //x = Ax + r * ((Bx - Ax)/sqrt((Bx - Ax)^2 + (By - Ay)^2))
        float x = _WaterSurface.position.x + _Radius * ((pointInCircle.x - _WaterSurface.position.x) / Mathf.Sqrt(Mathf.Pow(pointInCircle.x - _WaterSurface.position.x, 2) + Mathf.Pow(pointInCircle.z - _WaterSurface.position.z, 2)));
        //y = Ay + r * ((By - Ay)/sqrt((Bx - Ax)^2 + (By - Ay)^2))
        float y = _WaterSurface.position.z + _Radius * ((pointInCircle.z - _WaterSurface.position.z) / Mathf.Sqrt(Mathf.Pow(pointInCircle.x - _WaterSurface.position.x, 2) + Mathf.Pow(pointInCircle.z - _WaterSurface.position.z, 2)));

        _pointOnCircle = new Vector3(x, _WaterSurface.position.y, y);
        var pointDirection = (_pointOnCircle - _WaterSurface.position).normalized;
        _pointRightDirection = new Vector3(pointDirection.z, pointDirection.y, -pointDirection.x).normalized;
    }

    public void AddIngredient(Collider other)
    {
        if (other.gameObject.tag == "NaturalIngredient")
        {
            var ingredient = other.gameObject.GetComponent<NaturalIngredient>()._ingredient;

            if(_currentBlendDistance > 0f)
            {
                _isFirstBlend = false;
                _currentBlendDistance = 0f;
            }
            //for first item being added before blending
            if (_enteredIngredients.Count == 0 && _enteredPotions.Count == 0)
            {
                _totalCombinedBlending += _blendPercentage;
                _maxBlending++;
            }
            _isBlending = true;
            _currentBaseColor = _currentColor;
            _enteredIngredients.Add(ingredient);
            Destroy(other.gameObject);
            RuntimeManager.PlayOneShot(_WaterImpactSound, other.gameObject.transform.position);
        }
    }

    public void EnterPotion(int amount, PotionItem potion)
    {
        if (potion.Name.Equals("Water"))
            return;
        int index = _enteredPotions.FindIndex(x => x.Key.Id.Equals(potion.Id));
        if (_currentBlendDistance > 0f)
        {
            _isFirstBlend = false;
            _currentBlendDistance = 0f;
        }
        if (_enteredIngredients.Count == 0 && _enteredPotions.Count == 0)
        {
            _totalCombinedBlending += _blendPercentage;
            _maxBlending++;
        }
        _isBlending = true;
        _currentBaseColor = _currentColor;
        if (index >= 0)
        {
            _enteredPotions[index] = new KeyValuePair<PotionItem, int>(_enteredPotions[index].Key, _enteredPotions[index].Value + amount);
        }
        else
        {
            _enteredPotions.Add(new KeyValuePair<PotionItem, int>(potion, amount));
        }
    }
}
