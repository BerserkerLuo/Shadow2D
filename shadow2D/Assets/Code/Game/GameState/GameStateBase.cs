

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameStateBase
    {
        private bool m_bLoadFinish;

        public bool IsLoadFinish{ get { return m_bLoadFinish; }}

        protected virtual string SceneName {get{return "";}}

        public virtual IEnumerator LoadAsync(){
            if (string.IsNullOrEmpty(SceneName))
                yield break;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

            while (!asyncLoad.isDone) {
                //进度条之类的
                //float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }

            yield return null;
        }

        public virtual void OnEnter(){

            //CameraMgr.ReFindMainCamera();

            m_bLoadFinish = true;

            CursorManager.Instance.ChangeToBattleCurser();
        }

        public virtual void OnLeave(){ m_bLoadFinish = false;}
        public virtual void OnLaterLeave(){}

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
    }
}
