using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class BehaviourDecorator : IBehaviour
    {
        protected IBehaviour _childBehaviour;

        public BehaviourDecorator(IBehaviour childBehaviour)
        {
            _childBehaviour = childBehaviour;
        }

        public abstract override BehaviourState Execute(Blackboard blackboard);
    }

    public class Inverter : BehaviourDecorator
    {
        public Inverter(IBehaviour childBehaviour) :base(childBehaviour) { }

        public override BehaviourState Execute(Blackboard blackboard)
        {
            var state = _childBehaviour.Execute(blackboard);
            switch (state)
            {
                case BehaviourState.FAILURE:
                    return BehaviourState.SUCCESS;
                case BehaviourState.SUCCESS:
                    return BehaviourState.FAILURE;
                case BehaviourState.RUNNING:
                    return state;
                default:
                    break;
            }
            return BehaviourState.FAILURE;
        }
    }

    //https://www.researchgate.net/figure/The-repeat-until-failure-decorator_fig3_318679182
    public class RepeatUntilFail : BehaviourDecorator
    {
        //this should not be possible to ever hit, but just in case it will happen
        private uint _maxRepeatTimes = 100;
        private uint _nrOfVisits = 0;
        public RepeatUntilFail(IBehaviour childBehaviour) :base(childBehaviour) { }

        public override BehaviourState Execute(Blackboard blackboard)
        {
            BehaviourState state = BehaviourState.SUCCESS;
            while (state != BehaviourState.FAILURE)
            {
                if (_nrOfVisits >= _maxRepeatTimes)
                    return BehaviourState.FAILURE;

                state = _childBehaviour.Execute(blackboard);
                if (state != BehaviourState.SUCCESS && state != BehaviourState.FAILURE)
                    return state;

                _nrOfVisits++;
            }

            _nrOfVisits = 0;
            return BehaviourState.SUCCESS;
        }
    }
}
