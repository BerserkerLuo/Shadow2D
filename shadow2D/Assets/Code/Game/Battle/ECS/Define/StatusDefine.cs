using ECS.Script;

namespace ECS
{
    public class StatusInfo
    {
        public int StatusId;
        public int StatusLayer;
        public float LastTickTime;
        public float UnitTickTime;
        public float CreateTime;
        public float EndTime;
        public Entity FromEntity;
        public ScriptBase script;
        public bool SelfBreak = false;
    }
}
