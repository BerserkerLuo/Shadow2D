
using Tool;

namespace ECS
{
    public class SearchAreaType
    {
        public const int None   = 0;    //无目标
        public const int Single = 1;    //单体
        public const int Circle = 2;    //圆形

    }

    public class SearchInfo : ObjectPool<SearchInfo>
    {
        public Entity e;
        public float distance;
    }
}
