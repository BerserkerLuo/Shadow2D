
using System.Collections.Generic;
namespace ECS
{
    public delegate void SuspendCallBackFunction(ECSWorld logicWorld, SuspendData data);
    public class SuspendData
    {
        public SuspendCallBackFunction callBackFun;
        public int residueCount = 1;    //剩余触发次数
        public float intervalTime = 0f; //触发间隔
        public float nextTickTime = 0f; //
    }


    class SuspendComponent : Component
    {
        public int MakeSuspendId = 0;
        public Dictionary<int, SuspendData> DictSuspends = new Dictionary<int, SuspendData>();
        public Dictionary<int, SuspendData> AddDic = new Dictionary<int, SuspendData>();
    }
}

