
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    //===自动移动类型================================================================================================================
    public class AutoMoveType
    {
        public const int AutoMove_NoMove        = 0;    //不移动
        public const int AutoMove_Direction     = 1;    //按照方向移动
        public const int AutoMove_Pos           = 2;    //移动到目标点 
        public const int AutoMove_Path          = 3;    //路径移动
        public const int AutoMove_ToTarget      = 4;    //向目标移动
        public const int AutoMove_SurroundMove  = 5;    //环绕移动
        public const int AutoMove_FindPathToPos = 6;    //寻路到地点(带避障)
        public const int AutoMove_PickUpMove    = 7;    //拾取物品

    }

    //===自动移动基类================================================================================================================
    public class AutoMoveParamBase
    {
        public virtual int MoveType { get { return AutoMoveType.AutoMove_NoMove; } }

        //当前是否停止移动了
        //被标记停止移动后
        //    如果是临时移动 则转而执行基础移动 
        //    如果是基础移动 则不执行移动逻辑
        public bool IsStop = false;
        //是否移动到了目标点
        public bool IsArriveDestination = false;

        //是否暂停移动
        public bool IsPauseMove = false;

        //临时移动的开始时间和结束时间
        public float TempMoveStartTime = 0;
        public float TempModeEndTime = 0;

        //是否强制移动
        public bool IsForceMove = false;
        //是否使用强制移动速度
        public bool UseForceMoveSpeed = false;
        //强制移动速度
        public float ForceMoveSpeed = 15f;
        //是否面向移动点
        public bool IsFaceToTarget = true;
    }

    //===向某个方向移动================================================================================================================
    public class MoveToDirectParam : AutoMoveParamBase
    {
        public override int MoveType { get { return AutoMoveType.AutoMove_Direction; } }

        public Vector3 MoveDir = Vector3.zero;
    }

    //===向某个地点移动================================================================================================================
    internal class MoveToPosParam : AutoMoveParamBase
    {
        public override int MoveType { get { return AutoMoveType.AutoMove_Pos; } }

        public Vector3 TargetPos = Vector3.zero;
    }

    //===向某个单位移动================================================================================================================
    internal class MoveToTargetParam : AutoMoveParamBase{
        public override int MoveType { get { return AutoMoveType.AutoMove_ToTarget; } }

        public Entity TargetEntity;
        public Vector3 TargetPos = Vector3.zero;
        public float KeepDistance = 0;
    }

    //===向某个单位移动================================================================================================================
    internal class PickUpMoveParam : AutoMoveParamBase {
        public override int MoveType { get { return AutoMoveType.AutoMove_PickUpMove; } }

        public Vector3 TargetPos = Vector3.zero;
        public Entity TargetEntity;
    }

    //===按设定的路径移动================================================================================================================
    internal class PathMoveParam : AutoMoveParamBase
    {
        public override int MoveType { get { return AutoMoveType.AutoMove_Path; } }

        public List<Vector2> PathList;
    }

    //===环绕某个单位或者点移动================================================================================================================
    internal class SurroundMoveParam : AutoMoveParamBase
    {
        public override int MoveType { get { return AutoMoveType.AutoMove_SurroundMove; } }

        //环绕的对象
        public Entity TargetEntity;
        //环绕的目标点
        public Vector3 TargetPos;
        //角速度
        public float AngularVelocity = 0;
        //偏心距离
        public float EccentricDistance = 0;
        //当前旋转到的角度
        public Vector3 SurroundAngle;
    }
    //===寻路到目标移动================================================================================================================
    internal class FindPathTargetMoveParam : AutoMoveParamBase
    {
        public override int MoveType { get { return AutoMoveType.AutoMove_FindPathToPos; } }

        public Entity target;
        public float keepDistance;
        public List<Vector3> PathList;
        public bool InSearchPath = false;
    }

    //===寻路到地点移动================================================================================================================
    internal class FindPathPosMoveParam : AutoMoveParamBase {
        public override int MoveType { get { return AutoMoveType.AutoMove_FindPathToPos; } }

        public Vector2 pos;
        public float keepDistance;
        public List<Vector3> PathList;
        public bool InSearchPath = false;
    }
    
}
