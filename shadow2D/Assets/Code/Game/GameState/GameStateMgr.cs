using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tool;

namespace Game
{
    public enum EnumGameState
    {
        estate_Login,
        eState_Lobby,
        eState_Battle,
        eState_Max
    }

    public class GameStateMgr : SingletonBase<GameStateMgr>
    {
        public GameStateMgr()
        {
            m_dicAllState[EnumGameState.estate_Login] = new GameStateLogin();
            m_dicAllState[EnumGameState.eState_Lobby] = new GameStateLobby();
            m_dicAllState[EnumGameState.eState_Battle] = new GameStateBattle();
            m_dicAllState[EnumGameState.eState_Max] = new GameStateBase();
        }

        public Coroutine ChangeGameState(EnumGameState eGameState)
        {
            m_queueNewState.Enqueue(eGameState);
            if (null == m_coroutine)
                m_coroutine = UnityGameEntry.Instance.StartCoroutine(DoChangeGameState());

            return m_coroutine;
        }

        private IEnumerator DoChangeGameState()
        {
            while (m_queueNewState.Count > 0)
            {
                EnumGameState eGameState = m_queueNewState.Dequeue();

                GameStateBase gameState = null;
                if (false == m_dicAllState.TryGetValue(eGameState, out gameState))
                    continue;

                m_eGameStateGoto = eGameState;
                yield return UnityGameEntry.Instance.StartCoroutine(LeaveAsync());
                m_eGameStateOld = m_eGameState;
                m_eGameState = eGameState;
                m_gameStateCurrent = gameState;


                yield return UnityGameEntry.Instance.StartCoroutine(EnterAsync());

                try{
                    m_dicAllState[m_eGameStateOld].OnLaterLeave();
                }
                catch (Exception e){
                    Debug.LogError("Exception handling response: " + e.Message + " >>> " + e.Data["StackTrace"]);
                }

                yield return null;

                Resources.UnloadUnusedAssets();
            }
            m_coroutine = null;
        }

        private IEnumerator EnterAsync()
        {
            Coroutine coroutine = UnityGameEntry.Instance.StartCoroutine(m_gameStateCurrent.LoadAsync());
            yield return coroutine;

            try{
                m_gameStateCurrent.OnEnter();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private IEnumerator LeaveAsync(){
            try{
                m_gameStateCurrent.OnLeave();
            }
            catch (Exception e) {
                Debug.LogError("Exception handling response: " + e.Message + " >>> " + e.Data["StackTrace"]);
            }
            yield return null;
        }

        public void Update()
        {
            if (null != m_gameStateCurrent && m_gameStateCurrent.IsLoadFinish == true)
                m_gameStateCurrent.Update();
        }

        public void FixedUpdate()
        {
            if (null != m_gameStateCurrent && m_gameStateCurrent.IsLoadFinish == true)
                m_gameStateCurrent.FixedUpdate();
        }

        public void LateUpdate()
        {
            if (null != m_gameStateCurrent && m_gameStateCurrent.IsLoadFinish == true)
                m_gameStateCurrent.LateUpdate();
        }

        public EnumGameState GameState { get { return m_eGameState; } }

        private EnumGameState m_eGameStateGoto = EnumGameState.eState_Max;
        private EnumGameState m_eGameState = EnumGameState.eState_Max;
        private EnumGameState m_eGameStateOld = EnumGameState.eState_Max;
        private GameStateBase m_gameStateCurrent = new GameStateBase();
        private Dictionary<EnumGameState, GameStateBase> m_dicAllState = new Dictionary<EnumGameState, GameStateBase>();
        private Coroutine m_coroutine = null;
        private Queue<EnumGameState> m_queueNewState = new Queue<EnumGameState>();
    }
}
