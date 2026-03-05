
using Table;
using ECS.Script;
using System.Collections.Generic;

namespace ECS
{
    partial class StatusDataUtils
    {
        //创建新状态信息
        public static StatusInfo CreateNewStatus(Entity le,StatusCfg  statusCfg, Entity fromEntity = null) {
            StatusInfo statusInfo = new StatusInfo();
            statusInfo.StatusId = statusCfg.ID;
            statusInfo.StatusLayer = 1;
            statusInfo.FromEntity = fromEntity;

            if (statusCfg.StepTime > 0){
                statusInfo.UnitTickTime = statusCfg.StepTime;
                statusInfo.LastTickTime = LogicUtils.GetTime(le) - statusInfo.UnitTickTime;
            }
            else
                statusInfo.UnitTickTime = float.MaxValue;
            statusInfo.script = ScriptUtils.GetNewStatusScript(statusCfg.ID);

            UpdateStatusCreateTime(le,statusCfg,statusInfo);

            return statusInfo;
        }

        public static void UpdateStatusCreateTime(Entity le, StatusCfg statusCfg, StatusInfo statusInfo) {
            statusInfo.CreateTime = LogicUtils.GetTime(le);

            if (statusCfg.Duration > 0)
                statusInfo.EndTime = statusInfo.CreateTime + statusCfg.Duration;
            else
                statusInfo.EndTime = float.MaxValue;
        }

        //添加状态层数
        public static int AddStatusLayer(Entity le, int statusId, int layer = 1,Entity fromEntity = null)
        {
            StatusComponent statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return 0;

            StatusCfg statusCfg = TableMgr.Singleton.GetStatusCfg(statusId);
            if (statusCfg == null)
                return 0;

            int realLayer = layer;
            if (realLayer == 0)
                return 0;

            StatusInfo statusInfo = null;
            statusComp.m_dictStatus.TryGetValue(statusId, out statusInfo);
            if (statusInfo != null){
                int oldLayer = statusInfo.StatusLayer;
                statusInfo.StatusLayer += realLayer;
               
                if (statusInfo.StatusLayer > statusCfg.MaxLayer)
                    statusInfo.StatusLayer = statusCfg.MaxLayer;

                realLayer = statusInfo.StatusLayer - oldLayer;
                UpdateStatusCreateTime(le, statusCfg, statusInfo);

                if (realLayer != 0)
                    statusInfo.script.OnStatusAddLayer(le, statusId, realLayer);
            }
            else {
                statusInfo = CreateNewStatus(le,statusCfg, fromEntity);
                statusComp.m_dictStatus.Add(statusId,statusInfo);
                statusInfo.script.OnStatusBegin(le,statusId);
            }

            return realLayer;
        }

        //删除状态
        public static void DelStatus(Entity le,int statusId) {
            StatusComponent statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return;

            StatusInfo statusIfo = null;
            statusComp.m_dictStatus.TryGetValue(statusId, out statusIfo);
            if (statusIfo == null)
                return;

            statusIfo.script.OnStatusEnd(le,statusIfo);
            statusComp.m_dictStatus.Remove(statusId);
        }

        public static void SetStatusSelfBreak(Entity le, int statusId)
        {
            StatusComponent statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return;

            StatusInfo statusIfo = null;
            statusComp.m_dictStatus.TryGetValue(statusId, out statusIfo);
            if (statusIfo == null)
                return;

            statusIfo.SelfBreak = true;
        }

        //获取状态
        public static StatusInfo GetStatus(Entity le, int statusId) {
            StatusComponent statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return null;

            StatusInfo retInfo = null;
            statusComp.m_dictStatus.TryGetValue(statusId,out retInfo);
            return retInfo;
        }

        public static int GetLayer(Entity le, int statusId) {
            StatusInfo info = GetStatus(le, statusId);
            if (info == null)
                return 0;
            return info.StatusLayer;
        }

        //获取所有状态信息 只可用于遍历 不能更改 Dictionary 的内容
        public static Dictionary<int, StatusInfo> GetAllStatus(Entity le) {
            StatusComponent statusComp = le.GetComponentData<StatusComponent>();
            if (statusComp == null)
                return null;
            return statusComp.m_dictStatus;
        }
    }
}
