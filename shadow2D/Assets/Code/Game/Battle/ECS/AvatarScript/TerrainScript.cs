
using System.Collections.Generic;
using UnityEngine;
using ECS;

/// <summary>
/// 地形脚本
/// </summary>
public class TerrainScript : MonoBehaviour
{
    private static int makeGUID = 0;

    public int terrainGuId;
    public int m_iTerrainXmlId = 0;
    public List<string> TerrainParam = new List<string>();

    public float height = 0;
    public float width = 0;

    //Vector3 selfPos;
    //Vector3 colliderPos;

    private void Awake()
    {
        terrainGuId = makeGUID++;
        //selfPos = transform.position;
    }

    private void Start()
    {
        //后期看看怎么调用 暂用延时
        //Invoke("ShowTerrianInMinimap", 5f);
        //Debug.Log("地形触发：" + transform.name);
        //TerrainScriptUtils.OnTerrainInit(m_iTerrainXmlId, TerrainParam, transform);

    }

    public void InvokeInitMinimap()
    {
        Invoke("ShowTerrianInMinimap", 2f);
    }

    /// <summary> 将任务地形显示在小地图上 </summary>
    public void ShowTerrianInMinimap()
    {
        //DataTerrain cfg = DataTerrainManager.Instance.GetDataByID(m_iTerrainXmlId);
        //if (cfg == null)
        //    return;

        //if (cfg.QuestId.Count <= 0)
        //    return;

        //for (int i = 0; i < cfg.QuestId.Count; i++)
        //{
        //    UIUtils.UpdateTerrianMinimapPos(transform.position, terrainGuId, cfg.QuestId[i]);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("TerrainScript OnTriggerEnter " + m_iTerrainXmlId);

        if (!CheckCanEnter(other))
            return;

        ObjectCollideScript otherScript = other.GetComponent<ObjectCollideScript>();
        if (otherScript == null)
            return;

        Entity otherEntity = otherScript.entity;
        if (otherEntity == null)
             
            return;

        //DataTerrain data = DataTerrainManager.Instance.GetDataByID(m_iTerrainXmlId);
        //if (data == null)
        //    return;

        ////Debug.Log("进入任务地形：" + m_iTerrainXmlId+"   碰撞盒名字："+transform.name);
        ////colliderPos = other.ClosestPoint(selfPos);
        //TerrainUtils.OnEnter(otherEntity, m_iTerrainXmlId, TerrainParam, transform.position, height, width, gameObject, terrainGuId);

    }




    private void OnTriggerExit(Collider other)
    {
        ObjectCollideScript otherScript = other.GetComponent<ObjectCollideScript>();
        if (otherScript == null)
            return;

        Entity otherEntity = otherScript.entity;
        if (otherEntity == null)
            return;
        //Debug.Log("退出任务地形：" + m_iTerrainXmlId);
        //TerrainUtils.OnExit(otherEntity, m_iTerrainXmlId, TerrainParam);
    }

    private bool CheckCanEnter(Collider other)
    {

        if (m_iTerrainXmlId == 1002 || m_iTerrainXmlId == 1001)
        {
            Vector3 normal = transform.forward;
            Vector3 toOther = other.transform.forward;
            float angle = Vector3.Angle(toOther, normal);
            //Debug.Log("角度：" + angle);
            if (angle > 90)
                return false;
            else
                return true;
        }

        return true;
    }

}


