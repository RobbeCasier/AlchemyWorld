using TMPro;
using UnityEngine;

public class InformationWindow : MonoBehaviour
{
    [SerializeField] private float _MaxDistance;
    [SerializeField] private GameObject _Canvas;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _Name;
    [SerializeField] private TMP_Text _Type;
    [SerializeField] private TMP_Text _Effects;
    [SerializeField] private TMP_Text _Quality;

    [SerializeField] private LayerMask ignoreMask;
    private void Update()
    {
        var cam = Camera.main;
        if (cam == null) return;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _MaxDistance, ~ignoreMask))
        {
            Item item = null;
            if (hit.collider.gameObject.tag.Equals("Potion"))
            {
                var potion = hit.collider.gameObject.GetComponent<Potion>();
                item = potion._item;
                _Canvas.SetActive(true);
            }
            else if (hit.collider.gameObject.tag.Equals("NaturalIngredient"))
            {
                var ingredient = hit.collider.gameObject.GetComponent<NaturalIngredient>();
                item = ingredient._ingredient.Item;
                _Canvas.SetActive(true);
            }
            else
            {
                _Canvas.SetActive(false);
                return;
            }

            //name
            _Name.text = item.Name;
            //type
            switch (item.ItemType)
            {
                case ItemType.INGREDIENT:
                    _Type.text = "Ingredient";
                    break;
                case ItemType.POTION:
                    _Type.text = "Potion";
                    break;
                case ItemType.POWDER:
                    _Type.text = "Powder";
                    break;
                default:
                    break;
            }
            //effect
            if (item.Effects.Length == 0)
                _Effects.text = "None";
            else
            {
                _Effects.text = "";
                foreach (var effect in item.Effects)
                {
                    _Effects.text += effect.GetEffectName() + "<br>";
                }
            }
            //quality
            _Quality.text = item.Quality.ToString();
        }
        else
            _Canvas.SetActive(false);
    }


}
