using UnityEngine;
using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;
using System.Collections.Generic;
using ECS;

public class RVOTest : MonoBehaviour
{

    List<int> agents = new List<int>();
    List<Transform> visuals = new List<Transform>();

    void Start()
    {
        RVOMgr.Singleton.Init();

        CreateAgent(new Vector3(-5, 0, 0));
        CreateAgent(new Vector3(-5, 0.1f, 0));
        CreateAgent(new Vector3(-5.1f, 0, 0));
        CreateAgent(new Vector3(-5.1f, 0.2f, 0));
        CreateAgent(new Vector3(-5.1f, 0.1f, 0));
        CreateAgent(new Vector3(5, 3, 0));
        CreateAgent(new Vector3(5, 0, 0));
        CreateAgent(new Vector3(3, 0, 0));
    }

    int MakeAgentId = 0;
    void CreateAgent(Vector2 startPos,float time = 2.0f)
    {
        int agentId = RVOMgr.Singleton.CreateNewAgent(startPos, 0.5f, time);
        agents.Add(agentId);

        // 创建可视化球体
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(1, 1, 1);
        sphere.transform.position = startPos;
        visuals.Add(sphere.transform);
    }

    void Update()
    {
        UpdateRVO();

        if (Input.GetMouseButtonUp(1)){
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0; 

            foreach (var agent in agents)
                RVOMgr.Singleton.SetTarget(agent,worldPos, 2);

            DebugMgr.UpdateTargetPos(0, worldPos);
        }
    }

    public void UpdateRVO() {
        RVOMgr.Singleton.Update();

        //if (!RVOMgr.Singleton.UpdateSucc)
        //    return;

        // 将模拟结果应用到 GameObject 上
        for (int i = 0; i < agents.Count; i++)
        {
            int id = agents[i];
            Vector2 moveDelta = RVOMgr.Singleton.GetMoveDelta(id);

            float sqrMagnitude = moveDelta.sqrMagnitude * 1000000;
            if (sqrMagnitude < 0.001f)
                return;
            

            Transform visual = visuals[i];

            Vector2 currentPos = visual.position;
            currentPos += moveDelta;
            visual.position = new Vector3(currentPos.x, currentPos.y, 0);

            RVOMgr.Singleton.SetAgentPosition(id, currentPos);

            //Debug.Log(currentPos);
        }
    }
}
