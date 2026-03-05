
namespace ECS
{
    public class AutoMoveComponent : Component
    {
        //基础移动
        public AutoMoveParamBase BaseMoveParam = null;

        //临时移动
        public AutoMoveParamBase TempMoveParam = null;

        //同时执行另一个移动(比如在强制冲刺时受到击飞 这两种移动会同时作用)
        public AutoMoveParamBase SideAutoMove = null;

        //上一次Update是否在移动? 用于决定是否播放移动动画
        public int LastUpdateMove = 0; 
    }
}
