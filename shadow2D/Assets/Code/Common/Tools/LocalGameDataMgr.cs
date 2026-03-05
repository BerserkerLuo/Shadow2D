
using UnityEngine;

namespace Tool
{
    public class LocalGameDataMgr : SingletonBase<LocalGameDataMgr>
    {

        public LocalGameDataMgr()
        {
            Achieve = PlayerPrefs.GetString("Achieve", "");
        }

        public void SetAchieve(string v)
        {
            Achieve = v;
            PlayerPrefs.SetString("Achieve", v);
        }
        public string Achieve { get; private set; }
    }
}
