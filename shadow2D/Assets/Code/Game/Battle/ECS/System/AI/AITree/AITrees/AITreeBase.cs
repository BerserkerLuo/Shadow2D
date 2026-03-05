
namespace ECS
{
    public class AITree
    {
        public virtual void Tick(Entity e)
        {
            if (m_rootNode == null)
                return;

            m_rootNode.Run(e);

            return;
        }

        public ATNodeSelect m_rootNode = new ATNodeSelect();
    }
}
