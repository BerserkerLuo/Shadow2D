using System;

namespace PlayerSystemData
{
    public class GiverUtils
    {

        static public void ParseEquipRewardStr(string str, out int xmlId, out int rarity, out int level)
        {
            string[] strList = str.Split(",");
            if (strList.Length > 0) xmlId = Convert.ToInt32(strList[0]); else xmlId = 0;
            if (strList.Length > 1) rarity = Convert.ToInt32(strList[1]); else rarity = 1;
            if (strList.Length > 2) level = Convert.ToInt32(strList[2]); else level = 1;
        }
    }
}
