using AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    SHELF,
    CASHREGISTER,
    OTHER
}
public class Customer : MonoBehaviour
{
    [Header("Default Parameters")]
    [SerializeField] public NavMeshAgent _NavMeshAgent;
    [SerializeField] private float _Speed = 1.5f;

    [Header("Display")]
    [SerializeField] private Transform _WishList;
    [SerializeField] private TMP_Text _WishListText;

    private CustomerInfo _customerInfo;
    private Blackboard _blackboard;
    private IDecisionMaking _decisionMaking = null;

    private DefaultCustomerBehaviours _defaultBehaviours = new();

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        ReInitialize(); 
    }
    private void Update()
    {
        UpdateCustomer(Time.deltaTime);

        var playerPos = General.Instance.ActieCharacterStats.gameObject.transform.position;
        var direction = _WishList.transform.position - playerPos;
        direction.y= 0;
        direction.Normalize();
        _WishList.transform.forward= direction;
    }
    private void ReInitialize()
    {
        _customerInfo = new CustomerInfo();
        int shopLevel = General.Instance.ActieCharacterStats.ShopReputationLevel;
        CustomerInitData initData = General.Instance.CustomerInitData;
        GetRandomPotions(initData, shopLevel);
        GetRandomBudget(initData, shopLevel);
        InitializeBlackboard();
    }
    private void Initialize()
    {
        _customerInfo = new CustomerInfo();
        int shopLevel = General.Instance.ActieCharacterStats.ShopReputationLevel;
        CustomerInitData initData = General.Instance.CustomerInitData;
        GetRandomPotions(initData, shopLevel);
        GetRandomBudget(initData, shopLevel);
        InitializeNewBlackboard();
        InitializeTree();
    }
    private void InitializeTree()
    {
        //DECISION MAKING
        _decisionMaking = new BehaviourTree
        (
            _blackboard,
            new Selector
            (
                AddBehaviours
                (
                new Conditional(_defaultBehaviours.IsPerformingAction),
                new Sequence
                (
                    AddBehaviours
                    (
                    new Conditional(_defaultBehaviours.HasTarget),
                    new Selector
                    (
                        AddBehaviours
                        (
                        new Sequence
                        (
                            AddBehaviours
                            (
                            new Conditional(_defaultBehaviours.HasReachedTarget),
                            new Selector
                            (
                                AddBehaviours
                                (
                                new Sequence
                                (
                                    AddBehaviours
                                    (
                                    new Conditional(_defaultBehaviours.IsTargetShelf),
                                    new Selector
                                    (
                                        AddBehaviours
                                        (
                                        new RepeatUntilFail
                                        (
                                            new Sequence
                                            (
                                                AddBehaviours
                                                (
                                                new Conditional(_defaultBehaviours.HasNecessaryItemThatIsAffortable),
                                                new Action(_defaultBehaviours.Take)
                                                )
                                            )
                                        ))
                                    ),
                                    new Action(_defaultBehaviours.RemoveTarget)
                                    )
                                ),
                                new Sequence
                                (
                                    AddBehaviours
                                    (
                                        new Conditional(_defaultBehaviours.IsCashRegister),
                                        new Action(_defaultBehaviours.PutMoneyDown),
                                        new Action(_defaultBehaviours.RemoveTarget)
                                    )
                                ),
                                new Action(_defaultBehaviours.Despawn)
                                )
                            ))
                        ),
                        new Action(_defaultBehaviours.ChangeToSeek)
                        )
                    ))
                ),
                new Sequence
                (
                    AddBehaviours
                    (
                    new Conditional(_defaultBehaviours.HasPaid),
                    new Action(_defaultBehaviours.WaitForCashRegister),
                    new Action(_defaultBehaviours.Leave)
                    )
                ),
                new Sequence
                (
                    AddBehaviours
                    (
                    new Conditional(_defaultBehaviours.HasAllEffects),
                    new Action(_defaultBehaviours.GoToCashRegister),
                    new Action(_defaultBehaviours.ChangeToSeek)
                    )
                ),
                new Sequence
                (
                    AddBehaviours
                    (
                    new Action(_defaultBehaviours.FindShelf),
                    new Action(_defaultBehaviours.ChangeToSeek)
                    )
                ),
                new Sequence
                (
                    AddBehaviours
                    (
                    new Conditional(_defaultBehaviours.HasTakenSomething),
                    new Action(_defaultBehaviours.GoToCashRegister),
                    new Action(_defaultBehaviours.ChangeToSeek)
                    )
                ),
                new Action(_defaultBehaviours.Leave)
                )
            )
        );
    }
    private List<IBehaviour> AddBehaviours(params IBehaviour[] behaviours)
    {
        List<IBehaviour> list = new();
        list.AddRange(behaviours);
        return list;
    }

    private void GetRandomPotions(CustomerInitData data, int shopLevel)
    {
        _customerInfo._wantedPotions = new List<List<string>>();
        var maxPotion = Random.Range(1, data.repData[shopLevel - 1].MaxAmountOfPotions + 1);
        _WishListText.text = "";
        for (int i = 0; i < maxPotion; i++)
        {
            _WishListText.text = _WishListText.text + "Potion " + (i + 1).ToString() + ": ";
            int maxEffects = Random.Range(1, data.repData[shopLevel - 1].MaxEffects);
            var listEffects = new List<string>();
            for (int j = 0; j < maxEffects; j++)
            {
                listEffects.Add(Effect.GetRandomEffect());
                if (j < maxEffects - 1)
                    _WishListText.text = _WishListText.text + listEffects[j] + ", ";
                else
                    _WishListText.text = _WishListText.text + listEffects[j] + "<br>";

            }

            _customerInfo._wantedPotions.Add(listEffects);
        }
    }

    private void GetRandomBudget(CustomerInitData data, int shopLevel)
    {
        _customerInfo._budget = (uint)Random.Range(data.repData[shopLevel - 1].MinGold, data.repData[shopLevel - 1].MaxGold);
        _customerInfo._hasToSpend = 0;
    }

    private void InitializeBlackboard()
    {
        _blackboard.ChangeData("Customer", this);
        _blackboard.ChangeData("CustomerInfo", _customerInfo);
        Transform target = null;
        _blackboard.ChangeData("Target", target);
        _blackboard.ChangeData("FoundItem", new PotionItem());

        //list
        _blackboard.ChangeData("ToBeFound", _customerInfo._wantedPotions);
        _blackboard.ChangeData("VisitedShelfs", new List<int>());

        //bool
        _blackboard.ChangeData("IsPerformingAction", false);
        _blackboard.ChangeData("Paid", false);

        //enum
        _blackboard.ChangeData("TargetType", TargetType.OTHER);

        //Targets
        _blackboard.ChangeData("TargetedShelf", (Shelf)null);
        _blackboard.ChangeData("CashRegister", General.Instance.CashRegister);
    }

    private void InitializeNewBlackboard()
    {
        _blackboard = new Blackboard();
        _blackboard.AddData("Customer", this);
        _blackboard.AddData("CustomerInfo", _customerInfo);
        Transform target = null;
        _blackboard.AddData("Target", target);
        _blackboard.AddData("FoundItem", new PotionItem());

        //list
        _blackboard.AddData("ToBeFound", _customerInfo._wantedPotions);
        _blackboard.AddData("VisitedShelfs", new List<int>());

        //bool
        _blackboard.AddData("IsPerformingAction", false);
        _blackboard.AddData("Paid", false);

        //enum
        _blackboard.AddData("TargetType", TargetType.OTHER);

        //Targets
        _blackboard.AddData("TargetedShelf", (Shelf)null);
        _blackboard.AddData("CashRegister", General.Instance.CashRegister);
    }

    // Update is called once per frame
    public void UpdateCustomer(float deltaTime)
    {
        _decisionMaking?.Update(deltaTime);
    }

    public void Seek(Vector3 targetPosition)
    {
        _NavMeshAgent.destination= targetPosition;
        _NavMeshAgent.speed = _Speed * General.Instance.GameTime.TimeSpeed;
    }
}
