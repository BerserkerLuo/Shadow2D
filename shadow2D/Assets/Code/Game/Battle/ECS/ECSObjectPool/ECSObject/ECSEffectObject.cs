using Table;

namespace ECS
{
    public class ECSEffectObject : ECSBaseObject
    {
        static public ECSEffectObject GetByEffectName(string effectName) { return GetByPath<ECSEffectObject>(GetEffectPath(effectName)); }

        static public string GetEffectPath(string effectName) {

            PrefabsEffectCfg effectCfg = TableMgr.Singleton.GetPrefabsEffectCfg(effectName);
            if (effectCfg == null)
                return effectName;

            return effectCfg.EffectPath;
        }
    }
}
