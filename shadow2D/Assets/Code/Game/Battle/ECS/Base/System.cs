using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace ECS
{
    public interface ISystem
    {
    }

    public abstract class System : ISystem
    {
        public System() {}

        public ECSWorld EcsWorld { get { return m_ecsWorld; } }

        virtual public void Init(ECSWorld ecsWorld){ m_ecsWorld = ecsWorld; }
        virtual public void Update() { }
        virtual public void LateUpdate() { }
        virtual public void FixedUpdate() { }
   
        //本系统需要哪些 component 
        //通过或的形式 生成对应的掩码 （本系统需要的com掩码）
        public void RequireComponent(Type type){
            var cdtMask = EcsWorld.GetComponentTypeMask(type);

            //this.sysComponentMask |= (1L << cdtMask);
            this.sysComponentMask[cdtMask] = true;
        }

        public virtual void AddEntity(Entity e){
            entities.Add(e);
        }

        //移除
        public virtual void RemoveEntity(Entity e){
            entities.Remove(e);
        }

        //是否拥有
        public bool ContainEntity(Entity e){ return entities.Contains(e);}


        //检查掩码是否一致
        public bool CheckMask(BitArray128 entityComponentMask)
        {
            return sysComponentMask.Equals(entityComponentMask.BitAnd(sysComponentMask));
        }

        //获取system的 c掩码
        public BitArray128 GetComponentMask(){
            return this.sysComponentMask;
        }

        ///
        ECSWorld m_ecsWorld = null;

        //本sys的mask
        protected BitArray128 sysComponentMask = new BitArray128();

        //当前系统存在哪些 entity
        protected HashSet<Entity> entities = new HashSet<Entity>();

    }
}
