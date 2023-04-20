using System.Collections.Generic;

namespace AI
{
    public class IBlackboardField
    {
    }
    public class BlackboardField<T> : IBlackboardField
    {
        private T _data;

        public BlackboardField(T data)
        {
            _data = data;
        }

        public T GetData() { return _data; }
        public void SetData(T data) { _data = data; }

    }
    public sealed class Blackboard
    {
        private Dictionary<string, IBlackboardField> _blackboard = new Dictionary<string, IBlackboardField>();
        public bool AddData<T>(in string name, T data)
        {
            if (_blackboard.TryGetValue(name, out IBlackboardField result))
            {
                return false;
            }
            _blackboard.Add(name, new BlackboardField<T>(data));
            return true;
        }

        public bool ChangeData<T>(in string name, T data)
        {
            if (_blackboard.TryGetValue(name, out IBlackboardField result))
            {
                BlackboardField<T> p = (BlackboardField<T>)result;
                if (p != null)
                {
                    p.SetData(data);
                    return true;
                }
            }
            return false;
        }

        public bool GetData<T>(in string name, ref T data)
        {
            if (_blackboard.TryGetValue(name, out IBlackboardField result))
            {
                BlackboardField<T> p = result as BlackboardField<T>;
                if (p != null)
                {
                    data = p.GetData();
                    return true;
                }
            }
            return false;
        }
    }
}
