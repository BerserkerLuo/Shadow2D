

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class ECSWorld
    {
        public ECSWorld() {}

        virtual public void Init() { }

        public virtual void Clear(){}

        //注册本ecsworld
        //包含那些component组件
        public void RegComponent(Type type){
            if (s_dicComponentTypeMask.ContainsKey(type) == true)
                return;
            s_dicComponentTypeMask[type] = s_nCTMIdCount++;
        }

        //根据com 的 type 来获取对应的 码 
        public byte GetComponentTypeMask(Type type){
            byte value = 255;
            if (s_dicComponentTypeMask.TryGetValue(type, out value) == false){
                value = 255;
            }
            return value;
        }

        
        public void AddSystem(System s){
            m_sysList.Add(s);
        }

        public virtual void Update(){
            foreach (System s in m_sysList)
            {
                try{ s.Update();}
                catch (Exception e) {
                    Debug.LogError(e);
                }
            }

            //每次update所有 system之后 需要把产生变化的 entity 重新归类一下（system 所持有的的 entity的对象刷一遍）
            //变化的意思是 add remove 了 entity 或者 com 
            ExcuteEntityChange();
        }

        //LateUpdate
        public virtual void LateUpdate(){
            foreach (System s in m_sysList)
                s.LateUpdate();
        }

        //FixedUpdate
        public virtual void FixedUpdate(){
            foreach (System s in m_sysList)
                s.FixedUpdate();
        }

        //处理entity的变化 校准 sys持有的entity
        private void ExcuteEntityChange() {
 
            foreach (var e in modifiedEntities)
                VerifySystem(e);

            this.modifiedEntities.Clear();

            //destroy dead entities
            for (var i = 0; i < this.killedEntities.Count; i++){
                var e = this.killedEntities[i];
                RemoveFromSystem(e);
                RemoveEntity(e);
            };

            //clear dead entity queue
            this.killedEntities.Clear();
        }

        private void RemoveFromSystem(Entity e){
            var entityMask = e.ComponentMask;
            foreach (var system in this.m_sysList)
                if (system.CheckMask(entityMask))
                    system.RemoveEntity(e);
        }



        //校准sys 里 持有的entity
        private void VerifySystem(Entity e){
            foreach (var system in this.m_sysList){
                //当前是否有
                var oldAdded = system.ContainEntity(e);
                //未来是否有
                var willAdd = system.CheckMask(e.ComponentMask);

                //过去持有 而现在不持有 则需要删除
                if (oldAdded && !willAdd)
                    system.RemoveEntity(e);

                //过去不持有 而现在持有 则需要加入
                if (!oldAdded && willAdd)
                    system.AddEntity(e);
            };
        }

        public Entity GetBaseEntity(int nEnId) {
            Entity entity = null;
            m_dicEntity.TryGetValue(nEnId, out entity);
            return entity;
        }


        //创建
        public Entity OnCreateEntity(Entity e){
            m_dicEntity[e.Eid] = e;
            return e;
        }

        //数据改变
        public void ModifyEntityComponent(Entity e){
            if (modifiedEntities.Contains(e))
                return;
            this.modifiedEntities.Add(e);
        }

        //删除
        private void RemoveEntity(Entity e){
            m_dicEntity.Remove(e.Eid);
            e.Destory();
        }

        //当前不直接杀死 而是等update最后再remove
        public void KillEntity(Entity e){
            killedEntities.Add(e);
        }

        //获取一个新的EntityID
        public int CreateNewEntityId() { return ++m_nEntityIdCounter; }

        public float Time { get { return m_GameTime; } }
        public void UpdateGameTime(float now) { m_GameTime = now - m_StartTime; }

        public Dictionary<int, Entity> GetDicEntity() { return m_dicEntity; }
        //所有 entity
        private Dictionary<int, Entity> m_dicEntity = new Dictionary<int, Entity>();//存储所有的实体
        //死亡 等待删除的 entity
        private List<Entity> killedEntities = new List<Entity>();
        //所有被改变过的entity
        private HashSet<Entity> modifiedEntities = new HashSet<Entity>();

        //所有注册于此的sys
        private List<System> m_sysList = new List<System>();

        //所有注册于此世界的com
        private Dictionary<Type, Byte> s_dicComponentTypeMask = new Dictionary<Type, byte>();
        //com id 的掩码最大值
        private byte s_nCTMIdCount = 0;
        //游戏开始时间
        private float m_StartTime = 0;
        private float m_GameTime = 0;

        //全局Entity
        public Entity GlobalEntity;

        public int m_nEntityIdCounter = 0;

        public GameHook GameHook = new GameHook();
    }
}