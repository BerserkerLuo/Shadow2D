

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ECS
{
    public interface IECSObjectPool {
        public string BriefInfo();
        public void Preload(int count=10);
        public ECSBaseObject Get(); 
        public void Return(ECSBaseObject obj);
        public void ReturnAll();
        public void SetExpansion(int type, int param);
        
    }

    public class ECSObjectPool<T> : IECSObjectPool where T : ECSBaseObject , new()
    {
        private HashSet<ECSBaseObject> m_setUsePool = new HashSet<ECSBaseObject>();
        private Queue<ECSBaseObject> m_queWaitPool = new Queue<ECSBaseObject>();

        private int ExpansionType = 1; //1固定扩容 2百分比扩容
        private int Expansionparam = 10;

        private GameObject prefab;
        private Transform parentRoot; 
        private ObjectPoolScript objectPoolScript = null;

        public ECSObjectPool(GameObject prefab){

            this.prefab = prefab;
            this.parentRoot = new GameObject(prefab.name + "_Pool").transform;
             
            objectPoolScript = this.parentRoot.gameObject.AddComponent<ObjectPoolScript>();

            DebugUtils.DebugLog("{} ECSObjectPool Init Get Succ {}", prefab.name, objectPoolScript != null);
        }

        public string BriefInfo() {
            return "useIndex " + m_setUsePool.Count + " Length " + (m_queWaitPool.Count + m_setUsePool.Count);
        }

        public void SetPoolParent(Transform poolParent) {parentRoot.parent = poolParent;}

        //===================================================================
        public void SetExpansion(int type,int param) {
            ExpansionType = type;
            Expansionparam = param;
        }

        //===================================================================
        //对外接口

        //获取一个对象
        public ECSBaseObject Get(){

            if (m_queWaitPool.Count < 10)
                Preload();

            if (m_queWaitPool.Count <= 0)
                AddNewElement();

            ECSBaseObject ret = m_queWaitPool.Dequeue();

            m_setUsePool.Add(ret);

            objectPoolScript.OnGet();

            return ret;
        }

        //返回一个对象到池中
        public void Return(ECSBaseObject obj){
            if (m_setUsePool.Remove(obj) == false)
                return;

            if (obj.IsEmpty())
                return;

            obj.transform.SetParent(parentRoot);
            obj._SetActive(false);

            objectPoolScript.StartCoroutine(DelayReturn(obj));

            objectPoolScript.OnReturn();
        } 

        //重置对象池
        public void ReturnAll(){

            List<ECSBaseObject> objList = new List<ECSBaseObject>();
            foreach (var it in m_setUsePool)
                objList.Add(it);

            foreach (var it in objList)
                it.Destory();

            objectPoolScript.Clear();
        }

        //===================================================================
        //内部逻辑接口

        //新增对象
        private bool AddNewElement(){
            ECSBaseObject ecsObj = new T();
            ecsObj.transform = GameObject.Instantiate(prefab).transform;
            ecsObj.pool = this;
            ecsObj.transform.name = prefab.name;
            ecsObj.AfterInit();

            ecsObj.transform.SetParent(parentRoot);
            ecsObj._SetActive(false);
            ecsObj.ReSet();
            m_queWaitPool.Enqueue(ecsObj);

            objectPoolScript.OnAdd();
            return true;
        }

        //延迟一帧 在执行退回到对象池操作
        private IEnumerator DelayReturn(ECSBaseObject obj) {
            yield return new WaitForSeconds(0.5f);
            obj.ReSet();
            m_queWaitPool.Enqueue(obj);
        }

        //预加载
        private Coroutine nowCoroutine;
        public void Preload(int count = -1){
            if (nowCoroutine != null) 
                return;

            if (count == -1) {
                if (ExpansionType == 1) 
                    count = Expansionparam;
                if (ExpansionType == 2) 
                    count = (int)((m_queWaitPool.Count + m_setUsePool.Count) * (Expansionparam / 100f));
            } 
             
            if (count < 1) count = 10;

            if(objectPoolScript == null)
            DebugUtils.DebugLog("{} ECSObjectPool objectPoolScript Is null", prefab.name);

            nowCoroutine = objectPoolScript.StartCoroutine(DelayAddNewElement(count));
        }

        private IEnumerator DelayAddNewElement(int count){
            for (int i = 0; i < count; ++i) {
                yield return null;
                AddNewElement();
            }
            nowCoroutine = null;
        }
    }
}