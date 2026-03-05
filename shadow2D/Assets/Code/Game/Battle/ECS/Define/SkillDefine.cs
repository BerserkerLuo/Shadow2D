
using Table;
using UnityEngine;

namespace ECS
{

    public enum SkillTargetType { 
        Enemy = 0,
        Frend = 1,
        Self = 2,
    }

    public enum SKILLSTATE
    {
        Idle = 0,           //初始化为0 开始施法就切到1
        BeforShot = 1,      //前摇
        Shot = 3,           //出手
        End = 4,            //结束 
    }


    public class SkillInfo {
        public int skillId = 0;     //技能ID
        public int skillLevel = 0;  //技能等级
        public SkillCfg skillCfg;
        public SkillLvCfg skillLvCfg;
    }

    public class SkillCDUnit
    {
        public float cd;
        public int groupId;            //CD组
        public float startTime;          //cd开始时间
        public float endTime;            //cd结束时间
    }

    public class SkillProcPrevInfo
    {
        public int slotId = -1;
        public int skillId = -1;
        public int skilLevel = 0;
        public int targetEid = -1;
        public Vector2 dire = Vector2.zero;
        public float x = 0f;    
        public float y = 0f;
        public float z = 0f;
    }

    public class SkillUnit
    {
        public int skillUnitID = 0;
        public SKILLSTATE state = SKILLSTATE.Idle;

        public int ownerEid = -1;
        public int slotId = 0;
        public int skillId = 0;
        public int targetEid = -1;
        public Vector2 dire = Vector2.zero;
        public float x = 0f;
        public float y = 0f;
        //public float z = 0f;

        public float startTime;        //开始施法时间
        public float stateEndTime;     //当前阶段结束时间

        public SkillCfg skillCfg;
        public SkillLvCfg skillLvCfg;

    }
}
