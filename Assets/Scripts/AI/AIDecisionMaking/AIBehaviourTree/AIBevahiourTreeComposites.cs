using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    //------------------------------------------------------------
    //  BEHAVIOR TREE COMPOSITES
    //------------------------------------------------------------
    //--- COMPOSITE BASE ---
    public abstract class BehaviourComposite : IBehaviour
    {
        protected List<IBehaviour> _childrenBehaviours;
        public BehaviourComposite(List<IBehaviour> childrenBehaviours)
        {
            _childrenBehaviours = childrenBehaviours;
        }

        public abstract override BehaviourState Execute(Blackboard blackboard);
    }
    
    public class Selector : BehaviourComposite
    {
        public Selector(List<IBehaviour> childrenBehaviours) : base(childrenBehaviours)
        {
        }

        public override BehaviourState Execute(Blackboard blackboard)
        {
            foreach (var child in _childrenBehaviours)
            {
                _CurrentState = child.Execute(blackboard);
                switch (_CurrentState)
                {
                    case BehaviourState.FAILURE:
                        continue;
                    case BehaviourState.SUCCESS:
                        return _CurrentState;
                    case BehaviourState.RUNNING:
                        return _CurrentState;
                }
            }
            return _CurrentState = BehaviourState.FAILURE;
        }
    }

    public class Sequence : BehaviourComposite
    {
        public Sequence(List<IBehaviour> childrenBehaviours) : base(childrenBehaviours)
        {
        }

        public override BehaviourState Execute(Blackboard blackboard)
        {
            foreach (var child in _childrenBehaviours)
            {
                _CurrentState = child.Execute(blackboard);
                switch (_CurrentState)
                {
                    case BehaviourState.FAILURE:
                        return _CurrentState;
                    case BehaviourState.SUCCESS:
                        continue;
                    case BehaviourState.RUNNING:
                        return _CurrentState;
                    default:
                        _CurrentState = BehaviourState.SUCCESS;
                        return _CurrentState;
                }
            }
            return _CurrentState = BehaviourState.SUCCESS;
        }
    }

    public class PartialSequence : BehaviourComposite
    {
        private int _currentBehaviourIndex = 0;
        public PartialSequence(List<IBehaviour> childrenBehaviours) : base(childrenBehaviours)
        {
        }

        public override BehaviourState Execute(Blackboard blackboard)
        {
            while (_currentBehaviourIndex < _childrenBehaviours.Count)
            {
                _CurrentState = _childrenBehaviours[_currentBehaviourIndex].Execute(blackboard);
                switch (_CurrentState)
                {
                    case BehaviourState.FAILURE:
                        _currentBehaviourIndex = 0;
                        return _CurrentState;
                    case BehaviourState.SUCCESS:
                        ++_currentBehaviourIndex;
                        return _CurrentState = BehaviourState.RUNNING;
                    case BehaviourState.RUNNING:
                        return _CurrentState;
                }
            }
            _currentBehaviourIndex = 0;
            return _CurrentState = BehaviourState.SUCCESS;
        }
    }
}
