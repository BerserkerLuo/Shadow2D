

namespace ECS
{
    //并列节点
    //不管true 或者 false
    //都会执行
    //所以看上去有点像是并行电路
    //并且 永远返回 设定的返回值

    public class ATNodeParallel: ATNodeComposite {
        bool result = true;
        public ATNodeParallel(bool result = true) { this.result = result; }
        public override bool Run(Entity e){
            foreach (var n in m_listNode)
                n.Run(e);
            return result;
        }
    }
}
