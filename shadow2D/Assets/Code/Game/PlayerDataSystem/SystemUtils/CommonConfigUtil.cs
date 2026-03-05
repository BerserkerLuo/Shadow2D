using Table;
using System;

namespace PlayerSystemData
{
    internal class CommonConfigUtil
    {
        static private int getIntValue(string key, int defRet = -1)
        {
            //var Cfg = DataConfigManager.Instance.GetDataById(key);
            //if (Cfg == null)
            //    return defRet;

            //try{
            //    return Convert.ToInt32(Cfg.V);
            //}catch{
            //    // TODO 报错...
            //}

            return defRet;
        }

        static private float getFloatValue(string key, float defRet = default)
        {
            //var Cfg = DataConfigManager.Instance.GetDataById(key);
            //if (Cfg == null)
            //    return defRet;

            //try{
            //    return Convert.ToSingle(Cfg.V);
            //} catch {
            //    // TODO 报错...
            //}

            return defRet;
        }

        static private string getStringValue(string key, string defRet = "")
        {
            return defRet;

            //var Cfg = DataConfigManager.Instance.GetDataById(key);
            //if (Cfg == null)
            //    return defRet;

            //return Cfg.V;
        }

        static public bool UseNetBlock() {
            return getIntValue("UseNetBlock") != 0;
        }

        static public string GetNetBlockTime() {
            return getStringValue("NetBlockTime");
        }
    }
}
