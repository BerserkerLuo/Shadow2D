
using System.Collections.Generic;

namespace ECS
{


    //组合节点
    public class ATNodeComposite:ATNodeBase
    {
        public virtual void AddNode(ATNodeBase n)
        {
            m_listNode.Add(n);
        }

        protected List<ATNodeBase> m_listNode = new List<ATNodeBase>();
    }

}
