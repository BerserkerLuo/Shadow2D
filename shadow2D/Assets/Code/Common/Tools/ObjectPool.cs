
using System.Collections.Generic;

namespace Tool
{
    public class ObjectPool<T> where T : new()
    {
        //public delegate T PoolCreateNew();

        static protected Queue<T> WaitList = new Queue<T>();
        static protected HashSet<T> UseList = new HashSet<T>();

        static public int NewCount = 0;
        static public int WaitCount { get { return WaitList.Count; } }
        static public int UseCount { get { return UseList.Count; } }

        static T tempRet = default(T);
        public static T Get()
        {
            if (WaitList.Count < 1) { tempRet = new T(); ++NewCount; }
            else tempRet = WaitList.Dequeue();
            UseList.Add(tempRet);
            return tempRet;
        }

        public static void Return(T ret)
        {
            UseList.Remove(ret);
            WaitList.Enqueue(ret);
        }

        public static void ReturnAll()
        {
            foreach (var it in UseList)
                WaitList.Enqueue(it);
            UseList.Clear();
        }
    }
}
