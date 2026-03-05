using ECS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


public class ObjectCollideScript : MonoBehaviour
{

    public void Init(Entity e)
    {
        entity = e;
        Eid = e.Eid;
        ClearCollideCache();
    }

    public void ClearCollideCache()
    {
        foreach (var it in m_dictCollideInfos)
            CollideInfoPool.Return(it.Value);

        m_dictCollideInfos.Clear();
    }

    public Dictionary<long, CollideInfo> DictCollideInfos { get { return m_dictCollideInfos; } }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");

        OnCollideHit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter");

        //int layer = collision.collider.gameObject.layer;
        //if (layer == CommonDef.iObstacleLayer)
        //    OnCollideObstacle();

        OnCollideHit(collision.collider);
    }


    float triggerTime = 0;
    private void OnTriggerStay(Collider other)
    {
        triggerTime += Time.deltaTime;
        if (triggerTime >= 0.1f)//触发停留检测频率
        {
            OnCollideHit(other);
            triggerTime = 0;
        }
    }

    float collideTime = 0;
    private void OnCollisionStay(Collision collision)
    {
        collideTime += Time.deltaTime;
        if (collideTime >= 0.3f)//碰撞停留检测频率
        {
            Profiler.BeginSample("Real OnCollisionStay");
            //int layer = collision.collider.gameObject.layer;
            //if (layer == CommonDef.iObstacleLayer)
            //    CollideUtils.onCollideObstacle(entity);

            OnCollideHit(collision.collider);
            collideTime = 0;
            Profiler.EndSample();
        }
    }


    private void OnCollideHit(Collider other)
    {

        ObjectCollideScript otherScript = other.GetComponent<ObjectCollideScript>();
        if (otherScript == null)
            return;

        //Debug.Log("OnCollideHit " + Eid + " " + otherScript.Eid);

        TempCollideKey = UnityCollideUtils.OnMakeUniqueCollideKey(Eid, otherScript.Eid);
        if (m_dictCollideInfos.ContainsKey(TempCollideKey) == true)
            return;

        Entity ontherEntity = otherScript.entity;
        if (ontherEntity == null)
            return;

        CollideInfo info = CollideInfoPool.GetCollideInfo();
        info.collideKey = TempCollideKey;
        info.collider1 = entity;
        info.collider2 = ontherEntity;
        m_dictCollideInfos.Add(TempCollideKey, info);

    }

    public Entity entity = null;
    public int Eid = 0;

    public bool isInValid = false;

    private long TempCollideKey = 0;
    private Dictionary<long, CollideInfo> m_dictCollideInfos = new Dictionary<long, CollideInfo>();
}

