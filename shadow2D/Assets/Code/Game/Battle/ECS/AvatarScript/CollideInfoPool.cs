
using System.Collections.Generic;

namespace ECS
{
    public class CollideInfo
    {
        public long collideKey = 0;
        public Entity collider1 = null;
        public Entity collider2 = null;
    }


    static class CollideInfoPool
    {
        static private Queue<CollideInfo> m_queCollideInfoPool = new Queue<CollideInfo>();
        static public CollideInfo GetCollideInfo()
        {
            if (m_queCollideInfoPool.Count <= 0)
                m_queCollideInfoPool.Enqueue(new CollideInfo());
            return m_queCollideInfoPool.Dequeue();
        }

        static public void Return(CollideInfo info)
        {
            m_queCollideInfoPool.Enqueue(info);
        }

        static public void ReturnList(List<CollideInfo> infoList)
        {
            for (int i = 0; i < infoList.Count; ++i)
                Return(infoList[i]);
        }
    }
}
