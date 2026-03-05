
using UnityEngine;

namespace ECS
{
    public class MonsterSpawnSystem : System
    {
        public MonsterSpawnSystem(ECSWorld world)
        {
            Init(world);
        }

        float time = 5;

        public override void Update()
        {
            time += Time.deltaTime;
            if (time < 5f)
                return;
            time = 0;

            Vector3 pos = LogicUtils.GetSurroundPlayerPos(EcsWorld,40,32);
            CreateUtil.CreateMonster(EcsWorld, pos, 10);
        }
    }
}
