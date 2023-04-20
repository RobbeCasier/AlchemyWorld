using AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultCustomerBehaviours
{
    #region CONDITIONS
    public bool HasTarget(Blackboard blackboard)
    {
        Transform target = null;
        var dataAvailable = blackboard.GetData("Target", ref target);
        if (!dataAvailable || !target)
            return false;
        return true;
    }
    public bool HasReachedTarget(Blackboard blackboard)
    {
        Transform target = null;
        Customer customer = null;
        var dataAvailable = blackboard.GetData("Target", ref target) && blackboard.GetData("Customer", ref customer);

        if (!dataAvailable)
            return false;

        Vector2 customerPoint = new Vector2(customer.transform.position.x, customer.transform.position.z);
        Vector2 targetPoint = new Vector2(target.position.x, target.position.z);
        if (Vector2.Distance(customerPoint, targetPoint) < 0.25f)
            return true;
        return false;
    }
    
    public bool HasPaid(Blackboard blackboard)
    {
        bool hasPaid = false;
        var dataAvailable = blackboard.GetData("Paid", ref hasPaid);
        if (!dataAvailable || !hasPaid)
            return false;
        return true;
    }

    public bool HasAllEffects(Blackboard blackboard)
    {
        List<List<string>> toBeFound = new List<List<string>>();
        var dataAvailable = blackboard.GetData("ToBeFound", ref toBeFound);
        if (!dataAvailable)
            return false;

        if (toBeFound.Count > 0)
            return false;
        return true;
    }

    public bool HasNecessaryItemThatIsAffortable(Blackboard blackboard)
    {
        CustomerInfo info = new CustomerInfo();
        Shelf shelf = null;
        List<List<string>> toBeFound = new List<List<string>>();
        var dataAvailable = blackboard.GetData("TargetedShelf", ref shelf) 
            && blackboard.GetData("ToBeFound", ref toBeFound)
            && blackboard.GetData("CustomerInfo", ref info);
        if (!dataAvailable)
            return false;

        List<string> effects = new List<string>();
        //look through all potions
        foreach ( var potion in shelf.Potions)
        {
            if (potion == null) continue;
            effects.Clear();
            var temp = potion.Effects;
            foreach (var effect in temp)
            {
                effects.Add(effect.GetEffectName());
            }

            //look through my required potions
            foreach (var neededPotion in toBeFound)
            {
                //is it the same
                if (effects.OrderBy(x => x).SequenceEqual(neededPotion.OrderBy(x => x)))
                {
                    //is it affortable
                    if (info._hasToSpend + potion.Price < info._budget)
                    {
                        blackboard.ChangeData("FoundItem", potion);
                        return true;
                    }
                }                
            }
        }
        return false;
    }

    public bool HasTakenSomething(Blackboard blackboard)
    {
        CustomerInfo info = new CustomerInfo();
        blackboard.GetData("CustomerInfo", ref info);
        if (info._hasToSpend > 0)
            return true;
        return false;
    }

    public bool IsPerformingAction(Blackboard blackboard)
    {
        bool isInAction = false;
        var dataAvailable = blackboard.GetData("IsPerformingAction", ref isInAction);
        if (!dataAvailable)
            return false;
        return isInAction;
    }

    public bool IsTargetShelf(Blackboard blackboard)
    {
        TargetType type = TargetType.OTHER;
        var dataAvailable = blackboard.GetData("TargetType", ref type);
        if (!dataAvailable)
            return false;
        if (type == TargetType.SHELF)
            return true;
        return false;
    }

    public bool IsCashRegister(Blackboard blackboard)
    {
        TargetType type = TargetType.OTHER;
        var dataAvailable = blackboard.GetData("TargetType", ref type);
        if (!dataAvailable)
            return false;
        if (type == TargetType.CASHREGISTER)
            return true;
        return false;
    }
    #endregion

    #region BEHAVIOURSTATES
    public BehaviourState ChangeToSeek(Blackboard blackboard)
    {
        Customer customer = null;
        Transform target = null;
        var datatAvailable = blackboard.GetData("Customer", ref customer) && blackboard.GetData("Target", ref target);
        if (!datatAvailable)
            return BehaviourState.FAILURE;

        customer.Seek(target.position);
        return BehaviourState.SUCCESS;
    }

    public BehaviourState Despawn(Blackboard blackboard)
    {
        Customer customer = null;
        var dataAvailable = blackboard.GetData("Customer", ref customer);
        if (!dataAvailable)
            return BehaviourState.FAILURE;
        customer.gameObject.SetActive(false);
        return BehaviourState.SUCCESS;
    }

    public BehaviourState Take(Blackboard blackboard)
    {
        CustomerInfo info = new CustomerInfo();
        Shelf shelf = null;
        PotionItem item = null;
        List<List<string>> toBeFound = null;
        var dataAvailable =
            blackboard.GetData("TargetedShelf", ref shelf)
            && blackboard.GetData("FoundItem", ref item)
            && blackboard.GetData("CustomerInfo", ref info)
            && blackboard.GetData("ToBeFound", ref toBeFound);

        if (!dataAvailable)
            return BehaviourState.FAILURE;

        info._hasToSpend += item.Price;

        List<string> effects = new List<string>();
        var temp = item.Effects;
        foreach (var effect in temp)
        {
            effects.Add(effect.GetEffectName());
        }

        int indx = toBeFound.FindIndex(x => x.OrderBy(y => y).SequenceEqual(effects.OrderBy(y => y)));
        if (indx < 0)
            return BehaviourState.FAILURE;

        toBeFound.RemoveAt(indx);

        shelf.RemovePotion(item);

        blackboard.ChangeData("CustomerInfo", info);

        return BehaviourState.SUCCESS;
    }

    public BehaviourState RemoveTarget(Blackboard blackboard)
    {
        Transform target = null;
        blackboard.ChangeData("Target", target);

        return BehaviourState.SUCCESS;
    }

    public BehaviourState Leave(Blackboard blackboard)
    {
        blackboard.ChangeData("Target", General.Instance.LeavePoint);
        blackboard.ChangeData("TargetType", TargetType.OTHER);
        return BehaviourState.SUCCESS;
    }

    public BehaviourState FindShelf(Blackboard blackboard)
    {
        List<int> visitedShelfs = null;
        blackboard.GetData("VisitedShelfs", ref visitedShelfs);

        var shelfs = General.Instance.PotionShelfs;
        if (visitedShelfs.Count == 0)
        {
            int indx = Random.Range(0, shelfs.Count);
            blackboard.ChangeData("TargetedShelf", shelfs[indx]);
            blackboard.ChangeData("Target", shelfs[indx].TargetPoint);
            blackboard.ChangeData("TargetType", TargetType.SHELF);
            visitedShelfs.Add(indx);
            return BehaviourState.SUCCESS;
        }
        else
        {
            if (visitedShelfs.Count == shelfs.Count)
                return BehaviourState.FAILURE;
            List<Shelf> limitedShelfs = new List<Shelf>();
            limitedShelfs.AddRange(shelfs);
            foreach (var index in visitedShelfs)
            {
                limitedShelfs.Remove(shelfs[index]);
            }

            int indx = Random.Range(0, limitedShelfs.Count);
            indx = shelfs.FindIndex(x => x.Equals(limitedShelfs[indx]));
            blackboard.ChangeData("TargetedShelf", shelfs[indx]);
            blackboard.ChangeData("Target", shelfs[indx].TargetPoint);
            blackboard.ChangeData("TargetType", TargetType.SHELF);
            visitedShelfs.Add(indx);
            return BehaviourState.SUCCESS;
        }
    }

    public BehaviourState GoToCashRegister(Blackboard blackboard)
    {
        var cashRegister = General.Instance.CashRegister;
        blackboard.ChangeData("Target", cashRegister.TargetPoint);
        blackboard.ChangeData("TargetType", TargetType.CASHREGISTER);
        return BehaviourState.SUCCESS;
    }

    public BehaviourState PutMoneyDown(Blackboard blackboard)
    {
        CustomerInfo info = new CustomerInfo();
        blackboard.GetData("CustomerInfo", ref info);

        blackboard.ChangeData("Paid", true);
        General.Instance.CashRegister.AddMoneyOnCounter(info._hasToSpend);
        return BehaviourState.SUCCESS;
    }

    public BehaviourState WaitForCashRegister(Blackboard blackboard)
    {
        if (General.Instance.CashRegister.IsMoneyOnCounter)
            return BehaviourState.RUNNING;
        else
            return BehaviourState.SUCCESS;
    }
    #endregion
}
