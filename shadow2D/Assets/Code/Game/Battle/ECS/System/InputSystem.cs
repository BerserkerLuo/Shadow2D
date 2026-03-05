
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class InputSystem : System
    {
        public InputSystem(ECSWorld world) {
            Init(world);
        }

        public override void Update(){
            ProcessMove();

            //checkMouseButtonDown();

            TryUseWeaponSkill();

            TestSkill();

            TestFun();
            //ConrtorlMeetingPoint();

            //TryChangeScene(KeyCode.Alpha1, "WoodWorld");
            //TryChangeScene(KeyCode.Alpha2, "SeaWorld");
        }
        //=======================================================================

        private void TestFun() {
            if (!Input.GetKeyDown(KeyCode.C))
                return;

            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null)
                return;

            for (int i = 0; i < 100; ++i) {
                Vector2 pos = LogicUtils.GetSurroundPlayerPos(EcsWorld, 35, 35);
                CreateUtil.CreateItem(EcsWorld, 101, pos);
            }
        }

        //=======================================================================

        bool lastMove = false;
        private void ProcessMove() {
            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null)
                return;

            bool temp = TryMove(player);
            if (temp != lastMove) {
                if (temp) AnimationUtil.Walk(player);
                else AnimationUtil.StopMove(player);
                lastMove = temp;
            }
        }

        private int MoveFlag = 0;
        private Vector2 StopPos;
        private bool TryMove(Entity player)
        {
            float hInput = Input.GetAxisRaw("Horizontal");
            float vInput = Input.GetAxisRaw("Vertical");
            var dir = new Vector3(hInput, vInput, 0);

            if (!checkHaveInput())
                dir = Vector3.zero;

            int newFlag = dir == Vector3.zero ? 0 : 1;
            int checkFlag = MoveFlag << 1 | newFlag;
            MoveFlag = newFlag;

            if (checkFlag == 0)
                return false;

            if (checkFlag == 2){
                RVOUtil.SetAgentStop(player);
                //AnimationUtil.StopMove(player);
                return false;
            }

            AvatarComponent avComp = player.GetComponentData<AvatarComponent>();
            if (avComp == null)
                return false;

            //float speed = AttrUtil.GetSpeed(player);

            Vector2 targetPos = avComp.Position + dir.normalized * 10;

            RVOUtil.SetAgentTarget(player, targetPos);

            //avComp.Forward = dir.normalized; 
            //avComp.Position = avComp.Position + dir.normalized * speed * Time.deltaTime;

            AnimationUtil.Walk(player);

            AnimationUtil.SetMoveDire(player, dir);

            return true;
        }

        private bool checkHaveInput()
        {
            if (Input.GetKey(KeyCode.W)) return true;
            if (Input.GetKey(KeyCode.A)) return true;
            if (Input.GetKey(KeyCode.S)) return true;
            if (Input.GetKey(KeyCode.D)) return true;
            if (Input.GetKey(KeyCode.UpArrow)) return true;
            if (Input.GetKey(KeyCode.LeftArrow)) return true;
            if (Input.GetKey(KeyCode.DownArrow)) return true;
            if (Input.GetKey(KeyCode.RightArrow)) return true;
            return false;
        }

        //=======================================================================
        //选中物体
        private void checkMouseButtonDown() {
            if (!Input.GetMouseButtonUp(0))
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit tempHit;

            if (!Physics.Raycast(ray, out tempHit))
                return;

            GameObject clickedObject = tempHit.collider.gameObject;
            OwnerScript ownerScript = clickedObject.GetComponent<OwnerScript>();
            if (ownerScript == null)
                return;

            Debug.Log(ownerScript.Owner.Eid);
        }

        //=======================================================================
        //鼠标释放
        private void TryUseWeaponSkill() {
            if (!Input.GetMouseButtonUp(0))
                return;

            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null)
                return;
            WeaponUtil.OnUseWeaponSkill(player, GetMousePos());
        }

        private static readonly Plane ground = new Plane(Vector3.up, Vector3.zero);
        Vector2 GetMousePos(){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (ground.Raycast(ray, out float enter))   // 射线与平面是否相交
                return ray.GetPoint(enter);             // 相交点
            return ray.GetPoint(10);
        }
        //=======================================================================

        public void TestSkill() {
            TestSkill(KeyCode.Alpha1, 1010);
            TestSkill(KeyCode.Alpha2, 1020);
            TestSkill(KeyCode.Alpha3, 1030);

        }

        public void TestSkill(KeyCode code,int skillId) {
            if (!Input.GetKeyDown(code))
                return;
            Entity player = LogicUtils.GetPlayerEntity(EcsWorld);
            if (player == null)
                return;
            SkillUtils.CastSKillToPosBySkillId(player, skillId, GetMousePos());
        }

        //=======================================================================
        private void TryChangeScene(KeyCode code,string SceneName)
        {
            if (!Input.GetKeyDown(code))
                return;

            UIUtils.OnChangeScence(SceneName);

        }

    }
}
