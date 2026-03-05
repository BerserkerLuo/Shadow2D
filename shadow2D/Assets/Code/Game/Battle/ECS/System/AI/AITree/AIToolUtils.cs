
namespace ECS
{
    internal class AIToolUtils
    {
        public static ATNodeTernary CreateTernaryNode(ATNodeBase checkNode, ATNodeBase trueNode, ATNodeBase falseNode) {
            ATNodeTernary retNode = new ATNodeTernary();
            retNode.AddCheckNode(checkNode);
            retNode.AddTrueNode(trueNode);
            retNode.AddFalseNode(falseNode);
            return retNode;
        }

        public static ATNodeSequence CreateSequenceNode(params ATNodeBase[] nodes) {
            ATNodeSequence retNode = new ATNodeSequence();
            foreach (ATNodeBase node in nodes)
                retNode.AddNode(node);
            return retNode;
        }
    }
}
