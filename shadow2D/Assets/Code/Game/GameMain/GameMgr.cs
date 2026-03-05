
using Tool;
using PlayerSystemData;
using ECS;
using Table;
using UILib;

namespace Game
{
    public class GameMgr : SingletonBase<GameMgr>
    {

        public void Init()
        {
            //加载表格
            TableMgr.Singleton.LoadCfg();
            //Stream stream = CustomFileReader.OpenFile(@"csv.byte");
            //DataManager.Singleton.Init(stream);

            ECSObjectPoolMgr.SetExpansion(1, 10);


            //预加载资源
            //List<DataModel> modelList = DataModelManager.Instance.DataList;
            //foreach (var it in modelList){
            //    string modelPath = ECSModelObject.GetModelPath(it.ModelName);
            //    IECSObjectPool pool = ECSObjectPoolMgr.GetObjectPoolByPath<ECSGameObject>(modelPath);
            //    if (pool == null)
            //        continue;

            //    if (it.ExpansionType != 0)
            //        pool.SetExpansion(it.ExpansionType, it.Expansionparam);
            //    pool.Preload(it.PreLoadCount > 0 ? it.PreLoadCount : 10);
            //}

            //List<DataEffect> effectList = DataEffectManager.Instance.DataList;
            //foreach (var it in effectList){
            //    string effectPath = ECSEffectObject.GetEffectPath(it.EffectName);
            //    IECSObjectPool pool = ECSObjectPoolMgr.GetObjectPoolByPath<ECSGameObject>(effectPath);
            //    if (pool == null)
            //        continue;

            //    if (it.ExpansionType != 0)
            //        pool.SetExpansion(it.ExpansionType, it.Expansionparam);
            //    pool.Preload(it.PreLoadCount > 0 ? it.PreLoadCount : 10);
            //}

            SystemMgr.Singleton.Init();
            

            CameraMgr.ReFindMainCamera();
        }

        public void EnterWorld() {
            GameStateMgr.Singleton.ChangeGameState(EnumGameState.eState_Lobby);
        }

        //=================================================================================

        public void Update() {
            GameStateMgr.Singleton.Update();
            UIManager.Singleton.OnDlgUptate();
        }
        public void FixedUpdate() {
            GameStateMgr.Singleton.FixedUpdate();
        }
        public void LateUpdate() {
            GameStateMgr.Singleton.LateUpdate();
        }

        public void OnApplicationQuit() {
            SystemMgr.Singleton.SaveData();
            //NetworkManager.Singleton.SendDisconnect();
        }

    }
}
