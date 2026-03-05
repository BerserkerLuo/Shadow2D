

namespace ECS
{

    //顺序执行 
    //一般会先挂载条件判断
    //只要一个条件判断是false 则返回false
    //后面会挂载一系列（也可能就是一个）action
    //所有 action执行完毕之后 会返回 true

    public class ATNodeSequence: ATNodeComposite
    {
        public override bool Run(Entity e) {
            //从头到位遍历
            //这里我不清楚foreach是否保证顺序 
            //不过可以先这么整 有问题之后再说
            foreach ( var n in m_listNode)
                if(n.Run(e) == false)
                    return false;
            return true;
        }
    }

}
