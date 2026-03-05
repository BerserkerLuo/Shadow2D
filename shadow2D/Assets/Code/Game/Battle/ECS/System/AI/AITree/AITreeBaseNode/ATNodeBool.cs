

namespace ECS
{
    //逻辑节点 永远返回传入的值
    public class ATNodeBool : ATNodeBase
    {
        bool ret = false;
        public ATNodeBool(bool flag) {
            ret = flag;
        }
        public override bool Run(Entity e)
        {
            return ret;
        }
    }
}
