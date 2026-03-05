
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace ECS
{
    public class Entity
    {
        public int Eid;

        // 原文这里有entity type 不过我认为这是不需要的
        public ECSWorld EcsWorld { get { return m_ecsWorld; } }

        //本entity对应的component掩码
        public BitArray128 ComponentMask { get { return this.m_lComponentMask; } }

        public Entity(ECSWorld ew) { 
            m_ecsWorld = ew;
        }

        //这里最原著里 实际上有一个把com 返回对象池的操作 
        //目前我们暂时无视 之后有必要再加 其他复位其实没有实际意义 因为对象很快就会释放了
        public void Destory(){
            // free components            
            foreach (var component in componentDict.Values){
                component.UnInit();
            }
            componentDict.Clear();
            m_ecsWorld = null;
            Eid = 0;
        }


        //add component
        public T AddComponent<T>() where T : IComponent, new(){
            Type type = typeof(T);
            IComponent component = default(T);
            if (this.componentDict.TryGetValue(type, out component) == true && component.IsAlive == true){
                throw new Exception($"AddComponent, component already exist, Eid: {this.Eid}, component: {typeof(T).Name}");
            }
            if (component == null){
                component = new T();//ComponentFactory.CreateWithParent<K>(this, this.IsFromPool);
                this.componentDict.Add(type, component);
            }
            component.IsAlive = true;
            component.Init(this);

            //从当前ecs world 里 获取对应的 掩码
            byte cmtMask = m_ecsWorld.GetComponentTypeMask(typeof(T));
            m_lComponentMask[cmtMask] = true;
    
            m_ecsWorld.ModifyEntityComponent(this);
            return (T)component;
        }

        //获得某个component
        public T GetComponentData<T>() where T : IComponent
        {
            IComponent component;
            if (!this.componentDict.TryGetValue(typeof(T), out component))
                return default(T);
            return (T)component;
        }

        public bool RemoveComponent<T>() where T : IComponent{
            return RemoveComponent(typeof(T));
        }

        public bool RemoveComponent(Type t){

            IComponent componentData = null;
            if (componentDict.TryGetValue(t, out componentData) == false)
                return false;

            byte cmtMask = EcsWorld.GetComponentTypeMask(t);
            m_lComponentMask[cmtMask] = false;

            componentData.IsAlive = false;
            m_ecsWorld.ModifyEntityComponent(this);
            return true;
        }

        //所属ECS world
        private ECSWorld m_ecsWorld;

        // 所持有的的 com
        private Dictionary<Type, IComponent> componentDict = new Dictionary<Type, IComponent>();

        // com 列表生成的掩码 删选
        private BitArray128 m_lComponentMask = new BitArray128();
    }
}