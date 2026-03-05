
using System.Collections.Generic;
using System;

namespace ECS
{
    public class ModRegister
    {
        static Dictionary<string, Type> m_mods = new Dictionary<string, Type>();

        static ModRegister()
        {

        }

        static Type tempType;
        static public ModBase GetNewMod(string modName)
        {
            tempType = null;
            if (m_mods.TryGetValue(modName, out tempType) == false)
                return null;

            object myObject = Activator.CreateInstance(tempType);
            return (ModBase)myObject;
        }
    }
}
