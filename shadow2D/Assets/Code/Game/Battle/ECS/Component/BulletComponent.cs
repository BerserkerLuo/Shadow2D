
using System.Collections.Generic;

namespace ECS
{
    public class BulletComponent : Component 
    {
        //配置id
        public int BulletCfgID = 0;
        //到终点是否爆炸
        public bool IsEndPosExplode = true;
        //是否碰撞
        public bool IsCollider = true;
        //碰撞次数
        public int CollideCount = 1;
        //生命结束时间
        public float EndLifeTime = -1;
        //碰撞间隔
        public float LastCollideTime = 0;
        //最近碰到过的单位 <Eid,time>
        public Dictionary<int, float> dictCollideHistory = new Dictionary<int, float>();
    }
}
