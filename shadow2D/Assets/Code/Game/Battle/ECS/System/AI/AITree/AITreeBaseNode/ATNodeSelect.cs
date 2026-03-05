

namespace ECS
{
    //选择节点
    //选择接口
    //从左往右 或者说 从上往下选择 
    //遇到第一个节点true 返回true (有行为被选上了)
    //意义是根据有限级选择行为
    //如果所有节点都为false 则返回false (所有行为都没有被选上)
    public class ATNodeSelect: ATNodeComposite
    {

        public override bool Run(Entity e){
            foreach (var n in m_listNode)
                if (n.Run(e) == true)
                    return true;
            return false;
        }
    }

}
