
using System;
using System.Collections.Generic;
using Tool;

namespace ECS.Script
{
    class ScriptMgr : SingletonBase<ScriptMgr>
    {
        public static ScriptBase DefaultScript = new ScriptBase();
        private Dictionary<string, ScriptBase> m_ScriptDic = new Dictionary<string, ScriptBase>();

        private HashSet<string> InvaildList = new HashSet<string>();
        private Dictionary<string, Type> ScriptPrototypes = new Dictionary<string, Type>();

        public ScriptBase GetScript(string name)
        {
            ScriptBase script;
            bool succ = m_ScriptDic.TryGetValue(name, out script);
            if (succ)
                return script;

            script = CreateNewScript(name);
            m_ScriptDic[name] = script;

            return script;
        }

        public ScriptBase GetNewScript(string name)
        {
            return CreateNewScript(name);
        }

        private ScriptBase CreateNewScript( string name) {

            if (InvaildList.Contains(name))
                return default;

            ScriptBase script = CreateNewScript("ECS.Script.", name);
            //if(script == null)
            //    script = CreateNewScript("ECS.Script.", name);

            if (script == null){
                InvaildList.Add(name);
                return DefaultScript;
            }

            return script;
        }

        private ScriptBase CreateNewScript(string ScriptDomain, string name)
        {
            string scriptName = ScriptDomain + name;
            if (InvaildList.Contains(scriptName))
                return null;

            Type type = GetScriptPrototype(scriptName);
            if (type == null)
            {
                DebugUtils.Log("ScriptMgr CreateNewScript Faild ! script name =  " + scriptName);
                InvaildList.Add(scriptName);
                return null;
            }

            object obj = Activator.CreateInstance(type);
            if (obj == null)
            {
                InvaildList.Add(scriptName);
                DebugUtils.LogError("ScriptMgr CreateInstance Faild ! ScriptName =" + scriptName);
                return null;
            }

            ScriptBase script = (ScriptBase)obj;
            script.Init();

            return script;
        }

        private Type GetScriptPrototype(string scriptName) {
            Type type;
            if (ScriptPrototypes.TryGetValue(scriptName, out type))
                return type;

            type = Type.GetType(scriptName);
            if (type == null)
                return null;

            ScriptPrototypes.Add(scriptName,type);
            return type;
        }
    }
}
