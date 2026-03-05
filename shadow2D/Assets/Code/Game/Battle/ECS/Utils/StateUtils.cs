using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS
{
    partial class LogicUtils
    {
        public static void AddStateLayer(Entity le,int state,int layer = 1) {
            StateComponent stateComp = le.GetComponentData<StateComponent>();
            if (stateComp == null)
                return;

            int oldlayer;
            if (stateComp.m_dictStates.TryGetValue(state, out oldlayer) == false){
                stateComp.m_dictStates.Add(state, 1);
                return;
            }

            int newLayer = oldlayer + layer;
            if (newLayer < 0) newLayer = 0;

            stateComp.m_dictStates[state] = newLayer;
        }

        public static void DecStateLayer(Entity le, int state, int layer = 1)
        {
            StateComponent stateComp = le.GetComponentData<StateComponent>();
            if (stateComp == null)
                return;

            int oldlayer;
            if (stateComp.m_dictStates.TryGetValue(state, out oldlayer) == false)
            {
                //之前没有 则不可以扣除
                return;
            }

            int newLayer = oldlayer - layer;
            if (newLayer < 0) newLayer = 0;

            stateComp.m_dictStates[state] = newLayer;
        }


        public static int GetStateLayer(Entity le, int state)
        {
            StateComponent stateComp = le.GetComponentData<StateComponent>();
            if (stateComp == null)
                return 0;

            int layer;
            if (stateComp.m_dictStates.TryGetValue(state, out layer) == false)
                return 0;

            return layer;
        }

        public static void DelState(Entity le, eState stateType)
        {
            int layer = GetStateLayer(le, (int)stateType);
            if (layer <= 0)
                return;

            AddStateLayer(le, (int)stateType,-layer);
        }

        //无敌
        public static void SetGodState(Entity le) { AddStateLayer(le, (int)eState.eState_God); }
        public static void DecGodState(Entity le) { DecStateLayer(le, (int)eState.eState_God); }
        public static bool IsGodState(Entity le) { return GetStateLayer(le, (int)eState.eState_God) > 0; }

        //定身
        public static void SetRootState(Entity le) { 

            AddStateLayer(le, (int)eState.eState_Root); 
        }
        public static void DecRootState(Entity le) { DecStateLayer(le, (int)eState.eState_Root); }
        public static bool IsRootState(Entity le) { return GetStateLayer(le, (int)eState.eState_Root) > 0; }

        //冲锋
        public static void SetDashState(Entity le) { AddStateLayer(le, (int)eState.eState_Dash); }
        public static bool IsDashState(Entity le) { return GetStateLayer(le, (int)eState.eState_Dash) > 0; }

        //跳跃
        public static void SetJumpState(Entity le) { AddStateLayer(le, (int)eState.eState_Jump); }
        public static bool IsJumpState(Entity le) { return GetStateLayer(le, (int)eState.eState_Jump) > 0; }


        //吸附
        public static void SetAbsorbState(Entity le) { AddStateLayer(le, (int)eState.eState_Absorb); }
        public static void DecAbsorbState(Entity le) { DecStateLayer(le, (int)eState.eState_Absorb); }
        public static bool IsAbsorbState(Entity le) { return GetStateLayer(le, (int)eState.eState_Absorb) > 0; }

        //
        public static void SetBanSkillState(Entity le) { AddStateLayer(le, (int)eState.eState_BanSkill); }
        public static bool IsBanSkillState(Entity le) { return GetStateLayer(le, (int)eState.eState_BanSkill) > 0; }
    }
}
