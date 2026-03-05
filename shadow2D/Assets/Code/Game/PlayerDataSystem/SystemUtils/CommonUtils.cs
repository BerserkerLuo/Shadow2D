
using System.IO.Compression;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

namespace PlayerSystemData
{
    public class CommonUtils
    {
        /// <summary> 将 双分隔符 字符串解析成<int,int>字典 </summary>
        public static Dictionary<int, int> GetRewardItems(string str, string splitChar1 = "|", string splitChar2 = ";")
        {
            if (str == null || str == "")
                return null;
            string[] strList1 = str.Split(splitChar1);
            if (strList1.Length == 0)
                return null;

            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var item in strList1)
            {
                string[] strList2 = item.Split(splitChar2);
                if (strList2.Length < 2)
                    continue;
                int id = int.Parse(strList2[0]);
                //int num = int.Parse(strList2[1]);
                int num = (int)(float.Parse(strList2[1]));
                if (dic.ContainsKey(id))
                    dic[id] += num;
                else
                    dic.Add(id, num);
            }
            return dic;
        }

        public static string GetRewardStr(Dictionary<int,int> dic)
        {
            string retStr = "";
            if (dic == null || dic.Count == 0)
                return retStr;

            foreach(var pair in dic)
            {
                string tmpStr = "";
                if (retStr == "")
                {
                    tmpStr = pair.Key.ToString() + ";" + pair.Value.ToString();
                }
                else
                {
                    tmpStr = "|" + pair.Key.ToString() + ";" + pair.Value.ToString();
                }

                retStr += tmpStr;
            }

            return retStr;
        }


    }
}
