
namespace ECS
{
    public class AITreeHero : AITree
    {
        public AITreeHero() {

            m_rootNode.AddNode(new CheckIsDead());

            m_rootNode.AddNode(new ActionUseSkillToTarget());

            //是否操控移动
            //AddMoveControlNode();

            //是否警觉中
            //AddAlertNode();

            //是否疑惑中?
            //AddLossNode();

            //AddAutoUseSkill();

            //AddIdleNode();

            //查找敌人
            //ATNodeTernary searchTargetNode = new ATNodeTernary();
            //m_rootNode.AddNode(searchTargetNode);

            //searchTargetNode.AddCheckNode(new ActionAISearchEnemy());
            //    searchTargetNode.AddTrueNode(new ActionSetAIState(AIState.AI_Battle));

            ////回到驻守点
            //m_rootNode.AddNode(new ActionMoveToGuardPos());
        }

        public void AddMoveControlNode() {
            ATNodeTernary tempNode1 = AIToolUtils.CreateTernaryNode(
                new CheckArriveDestination(),
                new ActionSetAIState(AIState.AI_Idle),
                new ATNodeBool(true));

            ATNodeTernary MoveControlNode = AIToolUtils.CreateTernaryNode(
               new CheckIsInState(AIState.AI_MoveControl),
               tempNode1,null);

            m_rootNode.AddNode(MoveControlNode);
        }

        public void AddAutoUseSkill() {
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

        public void AddIdleNode() {
            //是否空闲状态?
            //  是 => 是否找到新敌人?
            //      是 => 切换到战斗状态 
            //      否 => 是否到达目标点?
            //          是 => 是否等待足够时间 ?
            //              是 => 移动到新游荡点
            //          否 => 保持状态

            ATNodeTernary IdleNode = new ATNodeTernary();
            ATNodeTernary SearchNewNode = new ATNodeTernary();
            ATNodeTernary IsArriveNode = new ATNodeTernary();
            ATNodeTernary IsWaitNode = new ATNodeTernary();
            ATNodeParallel ParalNode = new ATNodeParallel();

            m_rootNode.AddNode(IdleNode);

            IdleNode.AddCheckNode(new CheckIsInState(AIState.AI_Idle));
                IdleNode.AddTrueNode(SearchNewNode);
                SearchNewNode.AddCheckNode(new ActionAISearchEnemy());
                    SearchNewNode.AddTrueNode(new ActionSetAIState(AIState.AI_Battle));
                    SearchNewNode.AddFalseNode(IsArriveNode);
                    IsArriveNode.AddCheckNode(new CheckArriveDestination());
                        IsArriveNode.AddTrueNode(IsWaitNode);
                        IsWaitNode.AddCheckNode(new CheckStateTime(AIState.AI_Idle,8f));
                            ParalNode.AddNode(new ActionIdleRandMove());
                            ParalNode.AddNode(new ActionSetAIState(AIState.AI_Idle));
                            IsWaitNode.AddTrueNode(ParalNode);
                            IsWaitNode.AddFalseNode(new ATNodeBool(true));
                        IsArriveNode.AddFalseNode(new ATNodeBool(true));
        }

        public void AddAlertNode() {
            ATNodeTernary AlertNode = new ATNodeTernary();
            ATNodeTernary tempNode2 = new ATNodeTernary();

            m_rootNode.AddNode(AlertNode);
            AlertNode.AddCheckNode(new CheckIsInState(AIState.AI_Alert));
                AlertNode.AddTrueNode(tempNode2);
                    tempNode2.AddCheckNode(new CheckStateTime(AIState.AI_Alert, 0.1f));
                        tempNode2.AddTrueNode(new ActionSetAIState(AIState.AI_Battle));
                        tempNode2.AddFalseNode(new ATNodeBool(true));
        }

        public void AddLossNode() {
            ATNodeTernary LossNode = new ATNodeTernary();
            ATNodeTernary tempNode3 = new ATNodeTernary();
            ATNodeTernary tempNode4 = new ATNodeTernary();
            m_rootNode.AddNode(LossNode);

            LossNode.AddCheckNode(new CheckIsInState(AIState.AI_Loss));
                LossNode.AddTrueNode(tempNode3);
                    tempNode3.AddCheckNode(new ActionAISearchEnemy());
                        tempNode3.AddTrueNode(new ActionSetAIState(AIState.AI_Alert));
                        tempNode3.AddFalseNode(tempNode4);
                            tempNode4.AddCheckNode(new CheckStateTime(AIState.AI_Alert, 0.1f));
                                tempNode4.AddTrueNode(new ActionSetAIState(AIState.AI_Idle));
                                tempNode4.AddFalseNode(new ATNodeBool(true));
        }
    }


    //是否操控移动?
    //  是 => 是否到达?
    //      是 => 进入空闲状态
    //      否 => 保持状态
    //      
    //是否有敌对目标?
    //  是 => 目标是否死亡?
    //      是 => 是否有新敌人?
    //          是 => 尝试使用技能
    //          否 => 进入空闲状态
    //      否 => 尝试使用技能
    //
    //  尝试使用技能
    //      是否可以使用技能? (同时把要使用的技能记录到黑板)
    //          是 => 使用技能
    //          否 => 接近目标
    //
    //  接近目标
    //      是否距离目标过远? or 目标是否偏离既定目标点?
    //          是 => 朝目标移动
    //          否 => 保持状态   
    //
    //是否空闲状态?
    //  是 => 是否找到新敌人?
    //      是 => 切换到战斗状态 
    //      否 => 是否到达目标点?
    //          是 => 是否等待足够时间 ?
    //              是 => 移动到新游荡点
    //          否 => 保持状态
}
