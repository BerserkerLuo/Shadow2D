
using System.Collections.Generic;
using Table;

namespace ECS
{
    public class DropGroup{
        public float totalWeight = 0;
        public List<DropInfo> infos = new List<DropInfo>();
        public void AddInfo(DropInfo info) {
            infos.Add(info);
            totalWeight += info.weight;
        }
    }

    public class DropInfo
    {
        public float weight;
        public DropCfg dropCfg;
        public int p1;
        public int p2;
        public DropInfo Clone()
        {
            DropInfo tar = new DropInfo();
            tar.weight = weight;
            tar.dropCfg = dropCfg;
            return tar;
        }
    }

    partial class DropUtils
    {
     

        static Dictionary<int, DropGroup> DropGroupMap = new Dictionary<int, DropGroup>();
        public static DropGroup GetDropGroup(int groupId) {
            DropGroup group = DropGroupMap.GetValueOrDefault(groupId,null);
            if (group != null)
                return group;

            group = new DropGroup();

            List<DropCfg> allCfgs = TableMgr.Singleton.tables.DropCfgMgr.DataList;
            foreach (DropCfg cfg in allCfgs) {
                if (cfg.GroupId != groupId)
                    continue;
                DropInfo dropInfo = new DropInfo();
                dropInfo.weight = cfg.Weight;
                dropInfo.dropCfg = cfg;
                group.AddInfo(dropInfo);
            }

            DropGroupMap.Add(groupId, group);
            return group;
        }

        //=========================================================================================
        public static List<IntPair> ExecuteDrop(int dropGroupId, int times) {
            List<IntPair> retList = new List<IntPair>();
            for (int i = 0; i < times; ++i)
                retList.Add(ExecuteDrop(dropGroupId));
            return retList;
        }
        //执行掉落
        public static IntPair ExecuteDrop(int dropGroupId){
            DropGroup dropGroup = GetDropGroup(dropGroupId);
            int index = ExecuteDrop(dropGroup.infos, dropGroup.totalWeight);
            if (index == -1) return new IntPair(0,0);
            DropCfg dropCfg = dropGroup.infos[index].dropCfg;
            int randNum = LogicUtils.GetRand(dropCfg.MinNum, dropCfg.MaxNum);
            return new IntPair(dropCfg.ItemId, randNum);
        }

        public static int ExecuteDrop(List<DropInfo> Infos, float totalWeight)
        {
            if (Infos.Count < 1) return -1;
            if (Infos.Count < 2) return 0;

            DropInfo info;
            float rand = LogicUtils.GetRand(0, totalWeight);

            DebugUtils.Log($"ExecuteDrop rand {rand}");

            for (int index = 0; index < Infos.Count; ++index)
            {
                info = Infos[index];
                if (info.weight < 0.001f)
                    continue;

                if (rand <= info.weight)
                    return index;

                rand -= info.weight;
            }
            return 0;
        }

    }
}
