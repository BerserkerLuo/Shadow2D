
using System.Collections.Generic;
using Tool;

namespace ECS
{

    partial class AITreeType
    {
        public const int AITreeNone     = 0; //没有
        public const int AITreeMonster  = 1; //怪物
        public const int AITreeHero     = 2; //怪物
    }

    internal class AITreeMgr
    {
        static AITreeMgr() {
            RegisterAITree(AITreeType.AITreeMonster, new AITreeMonster());
            RegisterAITree(AITreeType.AITreeHero, new AITreeHero());
        }

        public static void RegisterAITree(int AITreeType, AITree aitree){
            if (m_dicBehaviourTree.ContainsKey(AITreeType) == false)
                m_dicBehaviourTree.Add(AITreeType, aitree);
            else{
                //让Mod 可以覆盖掉原来的AI行为树
                m_dicBehaviourTree[AITreeType] = aitree;
            }
        }

        public static AITree GetAITree(int type){
            AITree retAITree = null;
            m_dicBehaviourTree.TryGetValue(type, out retAITree);
            return retAITree;
        }

        private static Dictionary<int, AITree> m_dicBehaviourTree = new Dictionary<int, AITree>();
    }
}
