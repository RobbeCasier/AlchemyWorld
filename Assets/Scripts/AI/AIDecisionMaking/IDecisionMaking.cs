using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    abstract public class IDecisionMaking
    {
        public abstract void Update(float deltaTime);
    }
}
