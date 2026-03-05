
namespace ECS
{
    public class AITreeMonster : AITree
    {
        public AITreeMonster() {

            m_rootNode.AddNode(new CheckIsDead());

            AddAutoUseSkill();

            AddIdleNode();

            //ATNodeParallel NomalExecut = new ATNodeParallel(false);
            //NomalExecut.AddNode(new ActionUseSkillToTarget());
            //m_rootNode.AddNode(NomalExecut);

            ////是否有目标
            //ATNodeTernary checkHaveTarget = new ATNodeTernary();
            //checkHaveTarget.AddCheckNode(new CheckHaveTarget(SkillTargetType.Enemy));
            //m_rootNode.AddNode(checkHaveTarget);

            ////是否可以使用技能
            //ATNodeTernary checkCanUseSkill = new ATNodeTernary();
            //checkCanUseSkill.AddCheckNode(new CheckCanUseSkillToTarget());
            //checkHaveTarget.AddTrueNode(checkCanUseSkill);

            ////使用技能
            //ATNodeParallel userSkill = new ATNodeParallel(true);
            //userSkill.AddNode(new ActionStopMove());
            //userSkill.AddNode(new ActionUseSkillToTarget());
            //checkCanUseSkill.AddFalseNode(userSkill);

            ////移动到敌人附近
            //checkCanUseSkill.AddTrueNode(new ActionMoveToFollowTarget());

            ////查找敌人
            //ATNodeTernary searchTarget = new ATNodeTernary();
            //searchTarget.AddCheckNode(new ActionAISearchEnemy());
            //searchTarget.AddFalseNode(new ActionMoveToFollowTarget());
            //m_rootNode.AddNode(searchTarget);
        }

        public void AddAutoUseSkill()
        {
            //尝试使用技能
            ATNodeTernary HaveTargetNode = new ATNodeTernary();
            ATNodeTernary IsDeadNode1 = new ATNodeTernary();
            ATNodeTernary HaveNewTarget = new ATNodeTernary();
            ATNodeTernary TryAtkTarget = new ATNodeTernary();
            ATNodeTernary TryMoveToTarget = new ATNodeTernary();
            ATNodeParallel ParalNode = new ATNodeParallel();

            m_rootNode.AddNode(HaveTargetNode);
            HaveTargetNode.AddCheckNode(new CheckHaveTarget(SkillTargetType.Enemy));
            HaveTargetNode.AddTrueNode(IsDeadNode1);
            IsDeadNode1.AddCheckNode(new CheckTargetIsDead());
            IsDeadNode1.AddTrueNode(HaveNewTarget);
            IsDeadNode1.AddFalseNode(TryAtkTarget);

            HaveNewTarget.AddCheckNode(new ActionAISearchEnemy());
            HaveNewTarget.AddTrueNode(TryAtkTarget); 
            ParalNode.AddNode(new ActionLeaveBattle());
            ParalNode.AddNode(new ActionSetAIState(AIState.AI_Idle));
            HaveNewTarget.AddFalseNode(ParalNode);

            TryAtkTarget.AddCheckNode(new CheckCanUseSkillToTarget());
            TryAtkTarget.AddTrueNode(new ActionUseSkillToTarget());
            TryAtkTarget.AddFalseNode(TryMoveToTarget);

            TryMoveToTarget.AddCheckNode(new CheckCanMoveToTarget());
            TryMoveToTarget.AddTrueNode(new ActionMoveToFollowTarget());
            TryMoveToTarget.AddFalseNode(new ATNodeBool(true));
        }

        public void AddIdleNode()
        {
            //是否空闲状态?
            //  是 => 是否找到新敌人?
            //      是 => 切换到战斗状态 

            ATNodeTernary IdleNode = new ATNodeTernary();
            ATNodeTernary SearchNewNode = new ATNodeTernary();

            m_rootNode.AddNode(IdleNode);

            IdleNode.AddCheckNode(new CheckIsInState(AIState.AI_Idle));
            IdleNode.AddTrueNode(SearchNewNode);
            SearchNewNode.AddCheckNode(new ActionAISearchEnemy());
            SearchNewNode.AddTrueNode(new ActionSetAIState(AIState.AI_Battle));
        }
    }
}
