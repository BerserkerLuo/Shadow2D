
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tools;
using UnityEngine;

namespace ECS
{
    public class DebugUtils
    {
       

        public static void Log(params object[] paramsList) {
            Debug.Log(Util.merageLogStr(paramsList));
        }

        public static void LogError(params object[] paramsList)
        {
            Debug.LogError(Util.merageLogStr(paramsList));
        }

        public static void DebugLog(params object[] paramsList)
        {
            Debug.Log(Util.merageLogStr(paramsList));
        }
        
        //=====================================================================================================
        private static string AIThinkLog = "";
        private static bool showAIThink = false;
        public static void OnAIThink(params object[] paramsList) {
            if (!showAIThink) return;
            AIThinkLog = AIThinkLog + "\n" + Util.merageLogStr(paramsList) ;
        }
        public static void OnAIThinkEnd(int eId) {
            if (!showAIThink) return;
            DebugLog("AIThinkLog: eId {} {}",eId, AIThinkLog);
            AIThinkLog = "";
        }

        //=====================================================================================================

    }
}
