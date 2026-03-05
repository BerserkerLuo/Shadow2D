
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{

    public class ECSBaseObject
    {

        //==============================================================================
        #region

        static int typeMask = 0;
        static Dictionary<Type, int> dictTypeMasks = new Dictionary<Type, int>();
        static HashSet<int> setInvalidIds = new HashSet<int>();

        static Dictionary<Type, ECSBaseObject> dictbjectExamples = new Dictionary<Type, ECSBaseObject>();

        //生成检查码
        static private int GetCheckFlag(Type type ,int xmlId){
            if (dictTypeMasks.ContainsKey(type) == false) 
                dictTypeMasks.Add(type, ++typeMask);

            return (int)(xmlId << 8 | dictTypeMasks[type]);
        }

        static public T GetByXMLID<T>(int xmlId) where T : ECSBaseObject, new(){

            Type type = typeof(T);

            //根据类型和配置ID 生成 检查码
            int checkFlag = GetCheckFlag(type,xmlId);

            //如果检查码有失败过 就直接返回null
            if (setInvalidIds.Contains(checkFlag))
                return null;

            //如果不存在实例 就创建一个实例
            ECSBaseObject temp;
            if (dictbjectExamples.TryGetValue(type, out temp) == false){
                temp = new T();
                dictbjectExamples.Add(type, temp);
            }

            //从实例上拿到xmlId 的预制路径
            string path = temp.GetPath(xmlId);

            //通过路径去获取对象
            T ret = GetByPath<T>(path);

            //如果获取失败 则说明这个路径无效 把检查码加入无效列表
            if (ret == null) {
                setInvalidIds.Add(checkFlag);
                return null;
            }

            //设置 对象的xmlId
            ret.xmlId = xmlId;

            return ret;
        }

        static public T GetByPath<T>(string  path) where T : ECSBaseObject, new()
        {
            //从对象池管理器中获取对象池
            IECSObjectPool objPool = ECSObjectPoolMgr.GetObjectPoolByPath<T>(path);

            //如果没有会获取到对象池 说明路径无效 返回空对象
            if (objPool == null)
                return null;

            //从对象池中获取对象
            return (T)objPool.Get();
        }

        #endregion
        //==============================================================================

        public virtual void ReSet() {
            if (transform == null)
                return;

            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
            //transform.localScale = Vector3.one;
        }

        public virtual string GetPath(int xmlId) {
            Debug.LogError("GetPath Error ! ECSBaseObject GetPath!");
            return ""; 
        }

        public virtual void Destory(){
            if (isDestory)
                return;

            pool.Return(this);
            isDestory = true;
        }

        public virtual void OnActive() {
            _SetActive(true);
            isDestory = false;
        }

        public virtual void AfterInit() { }

        //==============================================================================

        public bool IsEmpty(){
            return transform == null || transform.gameObject == null;
        }

        public void _SetActive(bool active){
            if (IsEmpty())
                return;
            transform.gameObject.SetActive(active);
        }

        public bool IsActive() {
            if (IsEmpty())
                return false;
            return transform.gameObject.activeSelf;
        }

        //==============================================================================

        public int xmlId = 0;
        public Transform transform = null;
        public GameObject gameObject { get { return transform.gameObject; } }

        public IECSObjectPool pool = null;
        public bool isDestory = true;

        //==============================================================================
        public static Transform FindChild(Transform obj, string childName, int loop = 0){
            if (obj == null || loop > 5)
                return null;

            Transform[] childTransforms = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform childTransform in childTransforms){

                if (childTransform.name == childName)
                    return childTransform;

                Transform trans = FindChild(childTransform,childName,loop+1);
                if (trans != null)
                    return trans;
            }
            return null;
        }

        //==============================================================================
        //获取碰撞脚本
        private bool isGetCollideScript = false;
        private ObjectCollideScript m_CollideScript = null;
        public ObjectCollideScript GetCollideScript()
        {
            if (isGetCollideScript)
                return m_CollideScript;
            isGetCollideScript = true;

            m_CollideScript = transform.GetComponent<ObjectCollideScript>();
            return m_CollideScript;
        }

    }
}
