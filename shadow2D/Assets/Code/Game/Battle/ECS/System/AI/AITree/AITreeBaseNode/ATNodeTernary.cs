
namespace ECS
{


    //三元运算
    //  可以挂三个节点 分为 Center/Left/Right
    //  当 center节点 返回true时 执行 right节点 否则执行 left节点
    public class ATNodeTernary : ATNodeBase
    {
        public virtual void AddCheckNode(ATNodeBase n)
        {
            m_CheckNode = n;
        }

        public virtual void AddTrueNode(ATNodeBase n)
        {
            m_TrueNode = n;
        }

        public virtual void AddFalseNode(ATNodeBase n)
        {
            m_FalseNode = n;
        }

        public override bool Run(Entity e)
        {
            if (m_CheckNode == null)
                return false;

            ATNodeBase ExcuteNode = m_CheckNode.Run(e) ? m_TrueNode : m_FalseNode;
            if (ExcuteNode == null)
                return false;

            return ExcuteNode.Run(e);
        }

        private ATNodeBase m_CheckNode = null;
        private ATNodeBase m_TrueNode = null;
        private ATNodeBase m_FalseNode = null;
    }

}
