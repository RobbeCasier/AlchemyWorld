using System;

namespace AI
{
    public enum BehaviourState
    {
        FAILURE,
        SUCCESS,
        RUNNING
    }

    //------------------------------------------------------------
    //  BASE
    //------------------------------------------------------------
    public abstract class IBehaviour
    {
        public abstract BehaviourState Execute(Blackboard blackboard);

        protected BehaviourState _CurrentState = BehaviourState.FAILURE;
    }

    //------------------------------------------------------------
    //  CONDITIONAL
    //------------------------------------------------------------
    public sealed class Conditional : IBehaviour
    {
        private Func<Blackboard, bool> _condition;
        public Conditional(Func<Blackboard, bool> function)
        {
            _condition= function;
        }
        public override BehaviourState Execute(Blackboard blackboard)
        {
            if (_condition == null)
                return BehaviourState.FAILURE;

            if (_condition(blackboard))
                return _CurrentState = BehaviourState.SUCCESS;
            else
                return _CurrentState = BehaviourState.FAILURE;
        }
    }

    //------------------------------------------------------------
    //  ACTION
    //------------------------------------------------------------
    public sealed class Action : IBehaviour
    {
        private Func<Blackboard, BehaviourState> _action;
        public Action(Func<Blackboard, BehaviourState> function)
        {
            _action = function;
        }
        public override BehaviourState Execute(Blackboard blackboard)
        {
            if (_action == null)
                return BehaviourState.FAILURE;

            return _CurrentState = _action(blackboard);
        }
    }

    //------------------------------------------------------------
    //  BEHAVIOUR TREE
    //------------------------------------------------------------
    public sealed class BehaviourTree : IDecisionMaking
    {
        private BehaviourState _currentState = BehaviourState.FAILURE;
        private Blackboard _blackboard;
        private IBehaviour _rootComposite;
        public BehaviourTree(Blackboard blackboard, IBehaviour rootComposite)
        {
            _blackboard = blackboard;
            _rootComposite = rootComposite;
        }

        public override void Update(float deltaTime)
        {
            if (_rootComposite == null)
            {
                _currentState = BehaviourState.FAILURE;
                return;
            }
            _currentState = _rootComposite.Execute(_blackboard);
        }

        public Blackboard GetBlackboard()
        {
            return _blackboard;
        }
    }
}
