

using System.Collections.Generic;

namespace ECS
{
    public enum EcsEventType
    {
        OnUpDate = 1,    //Update事件
        AfterInit = 2,    //AfterInit事件

        CreateHeroEvent = 100,  //创建英雄
        CreateMonsterEvent = 101,  //创建怪物
        CreateBulletEvent = 102,  //创建子弹
        CreateDropBeanEvent = 103, //创建掉落物

        OnEntityDead = 110, //单位死亡
    }

    public interface EventParam { }
    public delegate void OnEventExcuteFun(EventParam param);

    public class EmptyEventPatam : EventParam { }

    public class GameHook
    {

        //================================================================================================================================
        //mod事件机制

        protected Dictionary<EcsEventType, List<OnEventExcuteFun>> m_DictEventList = new Dictionary<EcsEventType, List<OnEventExcuteFun>>();

        static public EmptyEventPatam EmptyParam = new EmptyEventPatam();

        public void RegEvent(EcsEventType eventType, OnEventExcuteFun fun)
        {
            List<OnEventExcuteFun> funList = GetEventFunList(eventType);
            funList.Add(fun);
        }

        public List<OnEventExcuteFun> GetEventFunList(EcsEventType eventType)
        {
            List<OnEventExcuteFun> funList = null;
            if (!m_DictEventList.TryGetValue(eventType, out funList))
            {
                funList = new List<OnEventExcuteFun>();
                m_DictEventList.Add(eventType, funList);
            }
            return funList;
        }

        public void ExcuteEvent(EcsEventType eventType, EventParam param)
        {
            List<OnEventExcuteFun> funList = GetEventFunList(eventType);
            foreach (var fun in funList)
                fun(param);
        }

        //================================================================================================================================
        //事件


    }
}
