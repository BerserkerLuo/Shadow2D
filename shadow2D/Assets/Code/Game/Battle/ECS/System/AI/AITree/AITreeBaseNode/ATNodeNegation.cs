
namespace ECS
{
    //逻辑非节点
    public class ATNodeNegation : ATNodeBase
    {
        ATNodeBase Node = null;
        public ATNodeNegation(ATNodeBase node) {
            Node = node;
        }
        

        public override bool Run(Entity e){
            if (Node == null)
                return false;

            return !Node.Run(e);
        }
    }

}
