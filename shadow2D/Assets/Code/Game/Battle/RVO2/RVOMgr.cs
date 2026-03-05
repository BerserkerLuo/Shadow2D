using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO{
    public class RVOMgr : SingletonBase<RVOMgr>
    {
        private Simulator simulator;

        private Dictionary<int, IAgent> agentMap = new Dictionary<int, IAgent>();
        private int MakeAgentId = 0;

        public void Init() {
            if (simulator != null) return;
            simulator = new Simulator(workers: 8, doubleBuffering: true, MovementPlane.XY);
            simulator.DesiredDeltaTime = 1.0f / 30;
        }

        public void Update() {
            simulator.Update();
        }

        public int CreateNewAgent(Vector2 bronPos,float radius,float agentTimeHorizon = 2,int layer = 0) {
            // 创建 Agent（2D坐标 + 高度）
            IAgent agent = simulator.AddAgent(bronPos, 0);

            agent.AgentId = ++MakeAgentId;
            // 设置 agent 参数 
            agent.Radius = radius;
            agent.AgentTimeHorizon = agentTimeHorizon; //预测和其他代理冲突的时间范围（单位：秒）。
            agent.ObstacleTimeHorizon = 2f;            //预测和静态障碍物冲突的时间范围。
            agent.MaxNeighbours = 10;
            agent.Layer = (RVOLayer)(1 << layer);


            agentMap.Add(agent.AgentId,agent);

            return agent.AgentId;
        }

        public void DelAgent(int agentId) {
            IAgent agent = agentMap.GetValueOrDefault(agentId, null);
            if (agent == null)
                return;

            simulator.RemoveAgent(agent);
            agentMap.Remove(agentId);
        }

        public void DllAllAgent() {
            foreach (var it in agentMap)
                simulator.RemoveAgent(it.Value);
            agentMap.Clear();
        }

        public void SetTarget(int agentId,Vector2 target,float speed) {
            IAgent agent = agentMap.GetValueOrDefault(agentId, null);
            if (agent == null)
                return;

            agent.SetTarget(target, speed, speed);
            agent.Locked = false;
        }

        public void SetLock(int agentId,bool look) {
            IAgent agent = agentMap.GetValueOrDefault(agentId, null);
            if (agent == null)
                return;
            agent.Locked = look;
        }

        public void SetAgentPosition(int id, Vector2 pos){
            var agent = agentMap.GetValueOrDefault(id);
            if (agent != null)
                agent.Position = pos;
        }

        public Vector2 GetMoveDelta(int agentId) {
            IAgent agent = agentMap.GetValueOrDefault(agentId, null);
            if (agent == null)
                return Vector2.zero;

            Vector2 direVec = (agent.CalculatedTargetPoint - agent.Position);
            Vector2 moveDelta = direVec.normalized * agent.CalculatedSpeed * Time.deltaTime;

            if(moveDelta.sqrMagnitude > direVec.sqrMagnitude)
                return direVec;
            return moveDelta;
        }

    }
}