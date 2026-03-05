using PlayerSystemData;
using ProjectX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using Game;
using Battle;
using Client.UI;
using Tools;

namespace ECS
{
    internal class UIUtils
    {
        public static ECSWorld GetECSWorld() {
            return BattleMgr.Singleton.GetEcsWorld();
        }

        public static Entity GetPlayer() {
            return LogicUtils.GetPlayerEntity(GetECSWorld());
        }

        //==============================================================================================================
        //切换场景

        static bool isChangeIng = false;        
        public static void OnChangeScence(string scenceName)
        {
            if (isChangeIng == true)
                return;
            isChangeIng = true;
            LogicUtils.StartCoroutine(LoadScene(scenceName));
        }

        public static IEnumerator LoadScene(string SceneName)
        {

            Debug.Log("PersonalUtils LoadScene:" + SceneName);
            if (string.IsNullOrEmpty(SceneName))
                yield return null;

            //float startTime = Time.time;

            //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

            //yield return ResourceMgr.Singleton.LoadScene(string.Format("Scenes/Map/{0}", SceneName));

            AsyncOperation asyncOper = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            yield return asyncOper;

            //CameraMgr.ReFindMainCamera();

            isChangeIng = false;
        }


        //==============================================================================================================
        public static void ShowDamageText(Vector3 pos,float damage,Vector3 damageDire) {
            DlgFlyText.singleton.ShowFlyText(pos, damage.ToString(),1, damageDire);
        }

        //==============================================================================================================

        public static void OnHPChange(Entity e)
        {
            if(LogicUtils.GetEntityType(e) == EnumEntityType.eHero)
                DlgMain.singleton.ShowHP();

            ECSGameObject healthBar = (ECSGameObject)AvatarDataUtil.GetHealthBar(e);
            if (healthBar == null)
                return;

            HealthBar script = healthBar.HealthBarScript;
            if (script == null)
                return;

            float maxHp = AttrUtil.GetHPMax(e);
            float hp = AttrUtil.GetHP(e);
            float fill = 1;
            if (maxHp != 0) fill = hp / maxHp;
            script.SetFill(fill);

        }

        public static void SetName(Entity e,string name) {
            ECSGameObject healthBar = (ECSGameObject)AvatarDataUtil.GetHealthBar(e);
            if (healthBar == null)
                return;

            HealthBar script = healthBar.HealthBarScript;
            if (script == null)
                return;

            script.SetName(name);
        }

        public static void SetClimpCameraPos(Vector2 minPos, Vector2 maxPos) {
            DlgGameControl.singleton.SetClimpCameraPos(minPos,maxPos);
        }

        public static void ShowBulletCount() {
            DlgMain.singleton.ShowBullet();
        }

        public static void ShowBulletReload(float time)
        {
            DlgMain.singleton.ShowReload(time);
        }

        public static void OnRefreshExp() {
            DlgMain.singleton.ShowExp();
        }

        public static void OnShowLevelUp(int addLv) {
            DlgLevelUp.singleton.OnLevelUp(addLv);
        }

        public static void OnRefreshCoin(int addCount) {
            DlgMain.singleton.OnCoinChange(addCount);
        }
    }
}
