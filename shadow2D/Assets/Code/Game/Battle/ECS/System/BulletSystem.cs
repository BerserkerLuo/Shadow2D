
using Table;
using System.Collections.Generic;
namespace ECS
{
    internal class BulletSystem : System
    {
        public BulletSystem(ECSWorld ecsWorld)
        {
            Init(ecsWorld);

            RequireComponent(typeof(BulletComponent));
        }

        public override void Update()
        {
            foreach (var e in entities){
                Tick(e);
            }
        }

        private void Tick(Entity e)
        {
            var com = e.GetComponentData<BulletComponent>();
            if (com == null)
                return;

            TryCollide(e, com);

            CheckToEndPos(e, com);

            CheckEndLife(e,com);

            //todo
            //检查离开边界很远的子弹
        }

        //===============================================================
        private void TryCollide(Entity e, BulletComponent bc)
        {
            if (bc.IsCollider == false)
                return;

            if (bc.CollideCount == 0)
                return;

            var bdata = TableMgr.Singleton.GetBulletCfg(bc.BulletCfgID);
            if (bdata == null)
                return;

            float Now = LogicUtils.GetTime(e);

            //检查公共cd
            if (Now - bc.LastCollideTime < bdata.CommonCD)
                return;

            bc.LastCollideTime = Now;

            List<Entity> resultList = SearchEnmey(bc);

            if (resultList.Count == 0)
                return;

            float CollideInterval = 0.5f;

            float entityLastCollideTime = 0f;
            foreach (Entity it in resultList)
            {
                if (bc.dictCollideHistory.TryGetValue(it.Eid, out entityLastCollideTime) == false)
                {
                    entityLastCollideTime = 0;
                    bc.dictCollideHistory.Add(it.Eid, Now);
                }

                //未过间隔时间
                if (Now - entityLastCollideTime < CollideInterval)
                    continue;

                //碰了 就要记下来
                bc.dictCollideHistory[it.Eid] = Now;
                bc.LastCollideTime = Now;

                _OnCollide(e, bc, bdata, it);

            }
        }

        private List<Entity> SearchEnmey(BulletComponent bc)
        {
            List<Entity> resultList = new List<Entity>();

            var bdata = TableMgr.Singleton.GetBulletCfg(bc.BulletCfgID);
            if (bdata == null)
                return resultList;

            //int atkrangetype = bdata.Attaa;
            //if (atkrangetype == SearchAreaType.Circle)
            resultList = SearchTargetUtils.SearchEnemyByRange(bc.Entity, AttrUtil.GetBodySize(bc.Entity));

            return resultList;
        }

        private void _OnCollide(Entity e, BulletComponent bc, BulletCfg bdata, Entity targetE)
        {
            if (bdata.CollideSkillID != 0)
                SkillUtils.CastSKillToTargetBySkillId(bc.Entity, bdata.CollideSkillID, targetE.Eid);

            //脚本
            ScriptUtils.OnBulletCollide(bc.BulletCfgID, bc.Entity, targetE);

            bc.CollideCount -= 1;

            if (bc.CollideCount == 0)
                LogicUtils.KillEntity(e);
        }

        //===============================================================
        private void CheckToEndPos(Entity e, BulletComponent bc)
        {
            //if (LogicUtils.IsDead(e)) return;
            if (bc.IsEndPosExplode == false) return;

            if (LogicDataUtils.IsArriveDestination(e) == false) return;

            UseBombSkill(e, bc);

            ScriptUtils.OnBulletArriveDestination(bc.BulletCfgID, bc.Entity);

            LogicUtils.KillEntity(e);
        }
        //===============================================================
        public void CheckEndLife(Entity e, BulletComponent bc) {
            if (bc.EndLifeTime == -1)
                return;

            if (bc.EndLifeTime > LogicUtils.GetTime(e))
                return;

            UseBombSkill(e, bc);

            LogicUtils.KillEntity(e);
        }
        //===============================================================
        public void UseBombSkill(Entity e, BulletComponent bc) {
            var pos = LogicUtils.GetPos(e);
            var bdata = TableMgr.Singleton.GetBulletCfg(bc.BulletCfgID);
            if (bdata != null && bdata.BombSkillId != 0)
                SkillUtils.CastSKillToPosBySkillId(e, bdata.BombSkillId, pos);
        }
    }
}
